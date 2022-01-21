using MemesProject.Data;
using MemesProject.Models;
using MemesProject.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace MemesProject.Controllers
{
    public class UserController : Controller
    {
        private readonly int PageSize = 5;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public UserController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public async Task<IActionResult> Index()
        {
              return View();
        }


  
        [HttpGet]
        public async Task<IActionResult> GetUserInformation(string id, int Page = 1)
        {
            //var applicationUser = _context.Users.FirstOrDefault(x => x.UserName.ToLower() == id.ToLower());
            var applicationUser = await _context.Users.FirstOrDefaultAsync(x => x.RealUserName.ToLower() == id.ToLower());  //uzyc gdy sie przesyla RealUserName
            if (applicationUser == null) 
            {
                return NotFound("Uzytkownik o nazwie "+id+" nie istnieje");
            }
            var userId = "";
            var isObservedDb= new Observation();
            if (User.Identity.IsAuthenticated)
            {
                var user = await _userManager.GetUserAsync(User);
                userId = await _userManager.GetUserIdAsync(user);
                if (userId == null)
                {
                    return NotFound("Uzytkownik o nazwie " + userId + " nie istnieje");
                }
                isObservedDb = await _context.Observations.FirstOrDefaultAsync(x => x.IdUser == userId && x.IdObservedUser == applicationUser.Id);
            }
            var observedUsersinfo = await _context.Observations.Where(x => x.IdUser == applicationUser.Id).Include(x=>x.ApplicationUser).Take(3).ToListAsync();
         
            UserInformation userInf = new UserInformation
            {
                Username = applicationUser.RealUserName,
                AccountRegisterDate = applicationUser.Account_Register_Date,
                AvatarImage = applicationUser.AvatarImage,
                IloscKomentarzy= await _context.Comments.Where(u => u.IdUser == id).CountAsync(),
                IloscMemow = await _context.Memes.Where(u => u.IdUser == id).CountAsync(),
                Email = applicationUser.Email,
                dateTimeLockout = applicationUser.LockoutEnd,
                memeViewModel = new MemeViewModel
                {
                    PagingInfo = new PagingInfo()
                    {
                        CurrentPage = Page,
                        ItemsPerPage = PageSize,
                        TotalItem = await _context.Memes.Where(m=>m.IdUser==id).CountAsync(),
                        urlParam = $"{id}?Page=:",
                    }
              } 
                
            };
            if (!User.Identity.IsAuthenticated)
            {
                var memes = await _context.Memes.Include(m => m.CategoryEntity).Where(m => m.IdUser == id).Skip((Page - 1) * PageSize).Take(PageSize).ToListAsync();
                userInf.memeViewModel.Memes = memes;
            }
            else
            {

                var memes =  await _context.Memes.Include(m => m.CategoryEntity).Where(m => m.IdUser == id).Skip((Page - 1) * PageSize).Take(PageSize).ToListAsync();


                var likeJoinQuery =
                from meme in memes
                join likedMeme in _context.LikedMemes on meme.IdMeme equals likedMeme.IdMeme
                where likedMeme.IdUser == userId

                select meme;

                var favouriteJoinQuery =
                from meme in memes
                join FavoritesMemes in _context.FavoritesMemes on meme.IdMeme equals FavoritesMemes.IdMeme
                where FavoritesMemes.IdUser == userId
                select meme;


                memes.ForEach(i =>
                {
                    if (likeJoinQuery.Any(c => c.IdMeme == i.IdMeme))
                    {
                        i.IsLiked = true;
                    }
                });


                memes.ForEach(i =>
                {
                    if (favouriteJoinQuery.Any(c => c.IdMeme == i.IdMeme))
                    {
                        i.IsFavourited = true;
                    }
                });
                userInf.memeViewModel.Memes = memes;
            }

            if (observedUsersinfo.Any())
            {
                userInf.Observers = new List<ObserverUserInfo>();
                foreach (var userDb in observedUsersinfo)
                {
                    ObserverUserInfo observerUser = new ObserverUserInfo();

                    observerUser.AvatarImage = userDb.ApplicationUser.AvatarImage;
                    observerUser.Username = userDb.ApplicationUser.RealUserName;
                  
                    userInf.Observers.Add(observerUser);
                }
            }
            if (User.Identity.IsAuthenticated)
            {
                if (isObservedDb != null)
                {
                    userInf.isObserved = true;
                }
            }

          
            return View(userInf);
 
        }


        [Authorize]
        public async Task<IActionResult> FollowUser(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var user = await _userManager.GetUserAsync(User);
            var userId = await _userManager.GetUserIdAsync(user);
          
            var applicationUser = await _context.Users.FirstOrDefaultAsync(x => x.RealUserName.ToLower() == id.ToLower());
            if (applicationUser == null)
            {
                return NotFound("Uzytkownik o nazwie " + id + " nie istnieje");
            }

            if (applicationUser.Id == userId)
            {
                return BadRequest();
            }
           
            Observation observation = new Observation();
            observation.IdUser = userId;
            observation.IdObservedUser = applicationUser.Id;
            _context.Observations.Add(observation);

            await _context.SaveChangesAsync();

            return RedirectToAction("GetUserInformation", new { id = id });

        }
        
        [Authorize]
        public async Task<IActionResult> UnFollowUser(string id)
        {

            if (id == null)
            {
                return NotFound();
            }
            var user = await _userManager.GetUserAsync(User);
            var userId = await _userManager.GetUserIdAsync(user);

            var applicationUser = await _context.Users.FirstOrDefaultAsync(x => x.RealUserName.ToLower() == id.ToLower());
            if (applicationUser == null)
            {
                return NotFound("Uzytkownik o nazwie " + id + " nie istnieje");
            }

            if (applicationUser.Id == userId)
            {
                return BadRequest();
            }

            var observation = await _context.Observations.FirstOrDefaultAsync(x => x.IdUser == userId && x.IdObservedUser == applicationUser.Id);
            if (observation == null)
            {
                return NotFound();
            }

            _context.Observations.Remove(observation);
            await _context.SaveChangesAsync();

            return RedirectToAction("GetUserInformation", new { id = id });

        }
        [Authorize]
        public async Task<IActionResult> FavouriteMemes(int Page = 1)
        {

            var claimsIdendity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdendity.FindFirst(ClaimTypes.NameIdentifier);

            MemeViewModel memeViewModel = new MemeViewModel();


            var memess = await _context.FavoritesMemes.Include(m => m.Meme).Where(x=>x.IdUser==claim.Value).Skip((Page - 1) * PageSize).Take(PageSize).ToListAsync();

            var likeJoinQuery =
            from meme in memess
            join likedMeme in _context.LikedMemes on meme.IdMeme equals likedMeme.IdMeme
            where likedMeme.IdUser == claim.Value
            select meme;


            memeViewModel.PagingInfo = new PagingInfo()

            {
                CurrentPage = Page,
                ItemsPerPage = PageSize,
                TotalItem = await _context.FavoritesMemes.Where(x => x.IdUser == claim.Value).CountAsync(),
                urlParam = "FavouriteMemes?Page=:",

            };
            memess.ForEach(i =>
            {
                if (likeJoinQuery.Any(c => c.IdMeme == i.IdMeme))
                {
                    i.Meme.IsLiked = true;
                }
            });


            memess.ForEach(i =>
            {
               
                    i.Meme.IsFavourited = true;
                
            });
            IList<Meme> memes = new List<Meme>();
            foreach (var meme in memess)
            {
                memes.Add(meme.Meme);
            }
            memeViewModel.Memes = memes;

            return View(memeViewModel);
        }
    }



}
