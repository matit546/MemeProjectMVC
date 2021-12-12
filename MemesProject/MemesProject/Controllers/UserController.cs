using MemesProject.Data;
using MemesProject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MemesProject.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        private readonly ApplicationDbContext _context;
        public UserController(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
              return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetUserInformation(string id)
        {
            var applicationUser = _context.Users.FirstOrDefault(x => x.UserName.ToLower() == id.ToLower());
            if (applicationUser == null) 
            {
                return NotFound();
            }
            UserInformation userInf = new UserInformation
            {
                Username = applicationUser.RealUserName,
                AccountRegisterDate = applicationUser.Account_Register_Date,
                AvatarImage = applicationUser.AvatarImage,
                //      IloscKomentarzy=_context.Comm
                IloscMemow = _context.Memes.Where(u => u.IdUser == id).Count(),
                Email = applicationUser.Email
            };

            return View(userInf);
 
        }
    }
}
