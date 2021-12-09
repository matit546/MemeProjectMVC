using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MemesProject.Models
{
    public class ApplicationUser : IdentityUser
    {
        public byte[] AvatarImage { get; set; }
        //public ICollection<Meme> MemesCollection { get; set; }
        //public ICollection<Observation> Observations { get; set; }
        //public ICollection<Comment> Comments { get; set; }
    }
}
