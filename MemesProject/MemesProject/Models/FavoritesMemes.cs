using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MemesProject.Models
{
    public class FavoritesMemes
    {
        public long IdFavoritesMemes { get; set; }
        public long IdMeme { get; set; }
        //public Meme Meme { get; set; }
        public string IdUser { get; set; }
    }
}
