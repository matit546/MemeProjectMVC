using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MemesProject.Models
{
    public class Meme
    {
        [Key]
        public long IdMeme { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string DescriptionAlt { get; set; }
        public string File { get; set; }
        public DateTime Date { get; set; }
        public int Likes { get; set; }
        //public LikedMemes LikedMemes { get; set; }
        //public FavoritesMemes FavoritesMemes { get; set; }
        public bool IfBlocked { get; set; }
        public bool IfApproved { get; set; }
        public long IdCategory { get; set; }
        //public ApplicationUser ApplicationUser { get; set; }
        public string IdUser { get; set; }
        public Category CategoryEntity { get; set; }
    }
}
