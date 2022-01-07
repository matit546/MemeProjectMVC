using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MemesProject.Data;
using MemesProject.Models;
using MemesProject.Helpers;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using MemesProject.Models.ViewModels;

namespace MemesProject.Controllers
{
    public class MemesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly int PageSize = 5;
        public MemesController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Memes
        public async Task<IActionResult> Index(int Page=1)
        {
            if (!User.Identity.IsAuthenticated)
            {
                MemeViewModel memesViewNotAuthorized = new MemeViewModel();
                var memes = await _context.Memes.Include(m => m.CategoryEntity).Skip((Page-1)*PageSize).Take(PageSize).ToListAsync();
    
                memesViewNotAuthorized.PagingInfo = new PagingInfo()
                {
                    CurrentPage = Page,
                    ItemsPerPage = PageSize,
                    TotalItem = await _context.Memes.CountAsync(),
                    urlParam = "Memes?Page=:",
                    
                };

                memesViewNotAuthorized.Memes = memes;
                return View(memesViewNotAuthorized);
            }



            var claimsIdendity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdendity.FindFirst(ClaimTypes.NameIdentifier);
            MemeViewModel memeViewModel = new MemeViewModel();



            var applicationDbContext =  await _context.Memes.Include(m => m.CategoryEntity).Skip((Page - 1) * PageSize).Take(PageSize).ToListAsync();
                

            var likeJoinQuery =
            from meme in applicationDbContext
            join likedMeme in _context.LikedMemes on meme.IdMeme equals likedMeme.IdMeme
            where likedMeme.IdUser==claim.Value
       
            select meme;

            var favouriteJoinQuery =
            from meme in applicationDbContext
            join FavoritesMemes in _context.FavoritesMemes on meme.IdMeme equals FavoritesMemes.IdMeme
            where FavoritesMemes.IdUser == claim.Value
            select meme;

            memeViewModel.PagingInfo = new PagingInfo()

            {
                CurrentPage = Page,
                ItemsPerPage = PageSize,
                TotalItem = await _context.Memes.CountAsync(),
                urlParam = "Memes?Page=:",

            };
            applicationDbContext.ForEach(i =>
            {
                if (likeJoinQuery.Any(c => c.IdMeme == i.IdMeme)){
                    i.IsLiked= true;
                }
            });


             applicationDbContext.ForEach(i =>
            {
                if (favouriteJoinQuery.Any(c => c.IdMeme == i.IdMeme))
                {
                    i.IsFavourited = true;
                }
            });

            memeViewModel.Memes = applicationDbContext;

            return View( memeViewModel);
        }

        // GET: Memes/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var meme = await _context.Memes
                .Include(m => m.CategoryEntity)
                .FirstOrDefaultAsync(m => m.IdMeme == id);





            if (meme == null)
            {
                return NotFound();
            }
            if (User.Identity.IsAuthenticated)
            {
                var user = await _userManager.GetUserAsync(User);
                var userId = await _userManager.GetUserIdAsync(user);
                var isLiked = await _context.LikedMemes.FirstOrDefaultAsync(x => x.IdMeme == id && x.IdUser == userId);
                var isFavourited = await _context.FavoritesMemes.FirstOrDefaultAsync(x => x.IdMeme == id && x.IdUser == userId);

                if (isLiked != null)
                {
                    meme.IsLiked = true;
                }
                if (isFavourited != null)
                {
                    meme.IsFavourited = true;
                }
            }
            return View(meme);
        }

        // GET: Memes/Create
        [Authorize]
        public IActionResult Create()
        {
            
            ViewData["IdCategory"] = new SelectList(_context.Categories, "IdCategory", "CategoryName");
            return View();
        }

        // POST: Memes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdMeme,Title,Description,DescriptionAlt,File,Date,IfBlocked,IfApproved,IdCategory,IdUser")] Meme meme, IFormFile file)
        {
            //var userName = User.FindFirstValue(ClaimTypes.Name);
            var user = await _userManager.GetUserAsync(User);
            meme.IdUser = user.RealUserName;

            meme.File = ImageChanger.ImageToBytes(file);
            var errors2 = ModelState.Values.SelectMany(v => v.Errors);
            meme.Date = DateTime.Now;
            if (ModelState.IsValid)
            {
                _context.Add(meme);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdCategory"] = new SelectList(_context.Categories, "IdCategory", "CategoryName", meme.IdCategory);
            return View(meme);
        }

        // GET: Memes/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var meme = await _context.Memes.FindAsync(id);
            if (meme == null)
            {
                return NotFound();
            }
            ViewData["IdCategory"] = new SelectList(_context.Categories, "IdCategory", "IdCategory", meme.IdCategory);
            return View(meme);
        }

        // POST: Memes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Edit(long id, [Bind("IdMeme,Title,Description,DescriptionAlt,File,Date,Likes,IfBlocked,IfApproved,IdCategory,IdUser")] Meme meme)
        {
            if (id != meme.IdMeme)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(meme);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MemeExists(meme.IdMeme))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdCategory"] = new SelectList(_context.Categories, "IdCategory", "IdCategory", meme.IdCategory);
            return View(meme);
        }

        // GET: Memes/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var meme = await _context.Memes
                .Include(m => m.CategoryEntity)
                .FirstOrDefaultAsync(m => m.IdMeme == id);
            if (meme == null)
            {
                return NotFound();
            }

            return View(meme);
        }

        // POST: Memes/Delete/5
        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var meme = await _context.Memes.FindAsync(id);
            _context.Memes.Remove(meme);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MemeExists(long id)
        {
            return _context.Memes.Any(e => e.IdMeme == id);
        }

        [Authorize]
        public async Task<IActionResult> LikeMeme(long? memeId)
        {
            if (memeId == null)
            {
                return RedirectToAction(nameof(Index));
            }
            var claimsIdendity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdendity.FindFirst(ClaimTypes.NameIdentifier);

            if (claim == null)
            {
                return NotFound();
            }

            var memeLike = _context.LikedMemes.FirstOrDefault(x => x.IdUser == claim.Value && x.IdMeme == memeId);
            if (memeLike == null)
            {
                LikedMemes LikedMeme = new LikedMemes();
                LikedMeme.IdUser = claim.Value;
                LikedMeme.IdMeme= (long)memeId;
                _context.LikedMemes.Add(LikedMeme);

                var memeLikePlus = await _context.Memes.FindAsync(memeId);
                memeLikePlus.Likes += 1;
               // _context.Memes.Attach(memeLikePlus);
                await _context.SaveChangesAsync();
                return Ok();
            }

            if(memeLike!=null)
            {
                _context.LikedMemes.Remove(memeLike);
                var memeLikePlus = await _context.Memes.FindAsync(memeId);
                memeLikePlus.Likes -= 1;
            //    _context.Memes.Update(memeLikePlus);
                await _context.SaveChangesAsync();
                return Ok();
            }

            return BadRequest();
        }

        [Authorize]
        public async Task<IActionResult> FavouriteMeme(long? memeId)
        {
            if (memeId == null)
            {
                return RedirectToAction(nameof(Index));
            }
            var claimsIdendity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdendity.FindFirst(ClaimTypes.NameIdentifier);

            if (claim == null)
            {
                return NotFound();
            }

            var memeFavourite = _context.FavoritesMemes.FirstOrDefault(x => x.IdUser == claim.Value && x.IdMeme == memeId);
            if (memeFavourite == null)
            {
                FavoritesMemes favouritedMeme = new FavoritesMemes();
                favouritedMeme.IdUser = claim.Value;
                favouritedMeme.IdMeme = (long)memeId;
                _context.FavoritesMemes.Add(favouritedMeme);
                await _context.SaveChangesAsync();
                return Ok();
            }

            if (memeFavourite != null)
            {
                _context.FavoritesMemes.Remove(memeFavourite);
                await _context.SaveChangesAsync();
                return Ok();
            }

            return BadRequest();
        }


        public async Task<IActionResult> Category(string? id, int Page=1)
        {
            if (!User.Identity.IsAuthenticated)
            {
                MemeViewModel memesViewNotAuthorized = new MemeViewModel();
                var memes = await _context.Memes.Include(m => m.CategoryEntity).Where(m => m.CategoryEntity.CategoryName == id).Skip((Page - 1) * PageSize).Take(PageSize).ToListAsync();

                memesViewNotAuthorized.PagingInfo = new PagingInfo()
                {
                    CurrentPage = Page,
                    ItemsPerPage = PageSize,
                    TotalItem = await _context.Memes.Where(m => m.CategoryEntity.CategoryName == id).CountAsync(),
                    urlParam = "Memes?Page=:",

                };

                memesViewNotAuthorized.Memes = memes;
                return View(memesViewNotAuthorized);
            }



            var claimsIdendity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdendity.FindFirst(ClaimTypes.NameIdentifier);
            MemeViewModel memeViewModel = new MemeViewModel();



            var memess = await _context.Memes.Include(m => m.CategoryEntity).Where(m => m.CategoryEntity.CategoryName == id).Skip((Page - 1) * PageSize).Take(PageSize).ToListAsync();


            var likeJoinQuery =
            from meme in memess
            join likedMeme in _context.LikedMemes on meme.IdMeme equals likedMeme.IdMeme
            where likedMeme.IdUser == claim.Value

            select meme;

            var favouriteJoinQuery =
            from meme in memess
            join FavoritesMemes in _context.FavoritesMemes on meme.IdMeme equals FavoritesMemes.IdMeme
            where FavoritesMemes.IdUser == claim.Value
            select meme;

            memeViewModel.PagingInfo = new PagingInfo()

            {
                CurrentPage = Page,
                ItemsPerPage = PageSize,
                TotalItem = await _context.Memes.Where(m => m.CategoryEntity.CategoryName == id).CountAsync(),
                urlParam = "Memes?Page=:",

            };
            memess.ForEach(i =>
            {
                if (likeJoinQuery.Any(c => c.IdMeme == i.IdMeme))
                {
                    i.IsLiked = true;
                }
            });


            memess.ForEach(i =>
            {
                if (favouriteJoinQuery.Any(c => c.IdMeme == i.IdMeme))
                {
                    i.IsFavourited = true;
                }
            });

            memeViewModel.Memes = memess;

            return View(memeViewModel);
        }
    }

   


}
