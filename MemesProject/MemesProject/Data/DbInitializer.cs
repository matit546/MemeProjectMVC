using MemesProject.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MemesProject.Data
{
    public class DbInitializer : IDbInitializer
    {

        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IWebHostEnvironment _env;
        public DbInitializer(ApplicationDbContext db, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager,
             IWebHostEnvironment env)
        {
            _db = db;
            _roleManager = roleManager;
            _userManager = userManager;
            _env = env;
        }
        public async Task Initialize()
        {         
            try
            {
                if(_db.Database.GetPendingMigrations().Count()>0)
                {
                    _db.Database.Migrate();
                }
            }catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            if (_db.Roles.Any(r => r.Name == ST.AdminRole))
            {
                return;
            }

            _roleManager.CreateAsync(new IdentityRole(ST.AdminRole)).GetAwaiter().GetResult();
            _roleManager.CreateAsync(new IdentityRole(ST.ModeratorRole)).GetAwaiter().GetResult();
            _roleManager.CreateAsync(new IdentityRole(ST.UserRole)).GetAwaiter().GetResult();
            byte[] data;
            using (var stream = new MemoryStream())
            {
                string webRootPath1 = _env.WebRootPath;
                string path = webRootPath1 + "\\images\\defaultImage.jpg";

                FileInfo fileInfo = new FileInfo(path);
                data = new byte[fileInfo.Length];

                using (FileStream fs = fileInfo.OpenRead())
                {
                    fs.Read(data, 0, data.Length);
                }          
            }
            _userManager.CreateAsync(new ApplicationUser
            {
                UserName = "admin@gmail.com",
                Email = "admin@gmail.com",
                EmailConfirmed = true,
                NormalizedUserName = "Administrator",
                PhoneNumber = "987654321",
                AvatarImage = data.ToArray()
            }, "Haslo1!").GetAwaiter().GetResult();

            ApplicationUser user = (ApplicationUser)await _db.Users.FirstOrDefaultAsync(u => u.Email == "admin@gmail.com");
            await _userManager.AddToRoleAsync(user, ST.AdminRole);
        }
    }
}
