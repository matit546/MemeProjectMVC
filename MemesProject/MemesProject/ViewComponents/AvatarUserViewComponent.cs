using MemesProject.Data;
using MemesProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Text;

namespace MemesProject.ViewComponents
{
    public class AvatarUserViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _db;

        public AvatarUserViewComponent(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            if (HttpContext.Session.GetString(ST.SessionUserName) == null)
            {
                var claimsIdendity = (ClaimsIdentity)User.Identity;
                var claim = claimsIdendity.FindFirst(ClaimTypes.NameIdentifier);
                var username = await _db.Users.Where(u => u.Id == claim.Value).Select(selector: u => u.RealUserName).FirstOrDefaultAsync();
                HttpContext.Session.SetString(ST.SessionUserName, username);
            }
       
      
            if (HttpContext.Session.GetString(ST.SessionImageAvatar) == null)
            {
                var claimsIdendity = (ClaimsIdentity)User.Identity;
                var claim = claimsIdendity.FindFirst(ClaimTypes.NameIdentifier);

   

                if (claim != null)
                {
                    byte[] ImageByte = await _db.Users.Where(u => u.Id == claim.Value).Select(selector: u => u.AvatarImage).FirstOrDefaultAsync();
                    HttpContext.Session.SetString(ST.SessionImageAvatar, Convert.ToBase64String(ImageByte));


                }
            }
            return View();

        
        }
    }
}
