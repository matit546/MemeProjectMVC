using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MemesProject.Models
{
    public class LikedMemes
    {
        [Key]
        public long IdLikedMemes { get; set; }
        public long IdMeme { get; set; }
        public virtual Meme Meme { get; set; }
        public string IdUser { get; set; }
    }
}
