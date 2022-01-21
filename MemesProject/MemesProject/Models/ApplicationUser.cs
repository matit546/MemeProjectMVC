using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MemesProject.Models
{
    public class ApplicationUser : IdentityUser
    {
        public byte[] AvatarImage { get; set; }
        public string Account_Register_Date { get; set; }

      
        public string RealUserName { get; set; }

        public string Role { get; set; }
        public virtual ICollection<Observation> Observations { get; set; }
        // zeby nie psuc loginu i rejestracji
        //public ICollection<Meme> MemesCollection { get; set; }
        //public ICollection<Observation> Observations { get; set; }
        //public ICollection<Comment> Comments { get; set; }
    }
}
