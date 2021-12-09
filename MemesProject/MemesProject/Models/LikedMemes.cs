using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MemesProject.Models
{
    public class LikedMemes
    {
        public long IdLikedMemes { get; set; }
        public int IdMeme { get; set; }
        //public Meme Meme { get; set; }
        public string User { get; set; }
    }
}
