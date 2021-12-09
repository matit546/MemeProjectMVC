using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MemesProject.Data;
using MemesProject.Models;

namespace MemesProject.Controllers
{
    public class MemesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MemesController(ApplicationDbContext context)
        {
            _context = context;
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
        public IActionResult Create()
        {
            ViewData["IdCategory"] = new SelectList(_context.Categories, "IdCategory", "IdCategory");
            return View();
        }

        // POST: Memes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdMeme,Title,Description,DescriptionAlt,File,Date,Likes,IfBlocked,IfApproved,IdCategory,IdUser")] Meme meme)
        {
            if (ModelState.IsValid)
            {
                _context.Add(meme);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdCategory"] = new SelectList(_context.Categories, "IdCategory", "IdCategory", meme.IdCategory);
            return View(meme);
        }

        // GET: Memes/Edit/5
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
