using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MemesProject.Models
{
    public class FavoritesMemes
    {
        [Key]
        public long IdFavoritesMemes { get; set; }
        public long IdMeme { get; set; }
        public virtual Meme Meme { get; set; }
        public string IdUser { get; set; }
    }
}
