using MemesProject.Data;
using MemesProject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MemesProject.Controllers
{
    public class UserController : Controller
    {
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
        public async Task<IActionResult> GetUserInformation(string id)
        {
            //var applicationUser = _context.Users.FirstOrDefault(x => x.UserName.ToLower() == id.ToLower());
            var applicationUser = await _context.Users.FirstOrDefaultAsync(x => x.RealUserName.ToLower() == id.ToLower());  //uzyc gdy sie przesyla RealUserName
            if (applicationUser == null) 
            {
                return NotFound("Uzytkownik o nazwie "+id+" nie istnieje");
            }
            var isObservedDb= new Observation();
            if (User.Identity.IsAuthenticated)
            {


                var user = await _userManager.GetUserAsync(User);
                var userId = await _userManager.GetUserIdAsync(user);
                 isObservedDb = await _context.Observations.FirstOrDefaultAsync(x => x.IdUser == userId && x.IdObservedUser == applicationUser.Id);
            }
            var observedUsersinfo = await _context.Observations.Where(x => x.IdUser == applicationUser.Id).Include(x=>x.ApplicationUser).Take(3).ToListAsync();
    
            UserInformation userInf = new UserInformation
            {
                Username = applicationUser.RealUserName,
                AccountRegisterDate = applicationUser.Account_Register_Date,
                AvatarImage = applicationUser.AvatarImage,
                //      IloscKomentarzy=_context.Comm
                IloscMemow = _context.Memes.Where(u => u.IdUser == id).Count(),
                Email = applicationUser.Email,
            };

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
    }
}
