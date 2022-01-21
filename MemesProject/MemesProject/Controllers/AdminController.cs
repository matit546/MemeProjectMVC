using MemesProject.Data;
using MemesProject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MemesProject.Controllers
{
    [Authorize(Roles =ST.AdminRole)]
    public class AdminController : Controller
    {
        
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        public AdminController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _context = context;
            _userManager= userManager;
            _signInManager = signInManager;
        }
        int PageSize = 10;
        public async Task<IActionResult> Index(int Page=1)            // user list
        {

            UserList UserList = new UserList();
            var usersList = await _context.Users.Skip((Page - 1) * PageSize).Take(PageSize).ToListAsync();

            UserList.PagingInfo = new PagingInfo()
            {
                CurrentPage = Page,
                ItemsPerPage = PageSize,
                TotalItem = await _context.Users.CountAsync(),
                urlParam = "?Page=:",

            };
     
            UserList.ApplicationUser = usersList;
            return View(UserList);
        }


        public async Task<IActionResult> LockUser(string id, int idpage)           
        {
            if (id == null)
            {
                return NotFound();
            }
            var applicationuser = await _context.Users.FirstOrDefaultAsync(x => x.Email == id);
            if(applicationuser == null)
            {
                return NotFound();
            }
            if (String.Equals(User.Identity.Name, applicationuser.Email))
            {
                return RedirectToAction(nameof(Index));
            }
            if (String.Equals(applicationuser.Email, "admin@gmail.com"))
            {
                return RedirectToAction(nameof(Index));
            }
            applicationuser.LockoutEnd = DateTime.Now.AddYears(100);
            await _context.SaveChangesAsync();
            if(idpage == 1)
            {
                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction("GetUserInformation", "User", new { id = applicationuser.RealUserName });
        }
        public async Task<IActionResult> UnLockUser(string id, int idpage)          
        {
            if (id == null)
            {
                return NotFound();
            }
            var applicationuser = await _context.Users.FirstOrDefaultAsync(x => x.Email == id);

            if (applicationuser == null)
            {
                return NotFound();
            }
            applicationuser.LockoutEnd = DateTime.Now;
            await _context.SaveChangesAsync();
            if (idpage == 1)
            {
                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction("GetUserInformation", "User", new { id = applicationuser.RealUserName });
        }
        public async Task<IActionResult> BlockMeme(int id)      
        {

            if (id == null)
            {
                return NotFound();
            }

            var meme = await _context.Memes.FirstOrDefaultAsync(x => x.IdMeme == id);

            if (meme == null)
            {
                return NotFound();
            }

            meme.IfBlocked = true;
            await _context.SaveChangesAsync();
            return Ok();


        }

        public   async Task<IActionResult> UnBlockMeme(int id)
        {

            if (id == null)
            {
                return NotFound();
            }

            var meme = await _context.Memes.FirstOrDefaultAsync(x => x.IdMeme == id);

            if (meme == null)
            {
                return NotFound();
            }

            meme.IfBlocked = false;
            await _context.SaveChangesAsync();
            return Ok();

        }

        public async Task<IActionResult> AddAdminRole(string id)
        {
            TempData["Message"] = "";
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users.FirstOrDefaultAsync(x => x.Email == id);
            if (user == null)
            {
                return NotFound();
            }
            if (String.Equals(user.Email, "admin@gmail.com"))
            {
                TempData["Message"] = "You cannot do that";
                return RedirectToAction(nameof(Index));
            }
            if (user == null)
            {
                return NotFound();
            }
            if (!String.Equals(User.Identity.Name, "admin@gmail.com"))
            {
                TempData["Message"] = "You cannot do that";
                return RedirectToAction(nameof(Index));
            }
            await _userManager.AddToRoleAsync(user, ST.AdminRole);
            user.Role = ST.ModeratorRole;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

        }

        public async Task<IActionResult> RemoveAdminRole(string id)
        {
            TempData["Message"] = "";
            if (id == null)
            {
                return NotFound();
            }
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Email == id);
            if (user == null)
            {
                return NotFound();
            }
            if (String.Equals(user.Email, "admin@gmail.com"))
            {
                return RedirectToAction(nameof(Index));
            }
            if (user == null)
            {
                return NotFound();
            }
            if (String.Equals(user.Email, User.Identity.Name))
            {
                await _userManager.RemoveFromRoleAsync(user, ST.AdminRole);
                user.Role = ST.UserRole;
                await _context.SaveChangesAsync();
                await _signInManager.RefreshSignInAsync(user);
            }

            if (!String.Equals(User.Identity.Name, "admin@gmail.com"))
            {
                TempData["Message"] = "You cannot do that";
                return RedirectToAction(nameof(Index));
            }

            await _userManager.RemoveFromRoleAsync(user, ST.AdminRole);
            user.Role = ST.UserRole;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

        }
    }
}
