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

namespace MemesProject.Controllers
{
    public class MemesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public MemesController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Memes
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Memes.Include(m => m.CategoryEntity);
            return View(await applicationDbContext.ToListAsync());
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
        public async Task<IActionResult> Create([Bind("IdMeme,Title,Description,DescriptionAlt,File,Date,Likes,IfBlocked,IfApproved,IdCategory,IdUser")] Meme meme, IFormFile file)
        {
            //var userName = User.FindFirstValue(ClaimTypes.Name);
            var user = await _userManager.GetUserAsync(User);
            meme.IdUser = user.RealUserName;

            meme.File = ImageChanger.ImageToBytes(file);

            meme.Date = DateTime.Now;
            //if (ModelState.IsValid)
            //{
                _context.Add(meme);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            //}
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
    }
}
