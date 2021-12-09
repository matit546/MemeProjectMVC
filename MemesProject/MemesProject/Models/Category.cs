using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MemesProject.Models
{
    public class Category
    {
        [Key]
        public long IdCategory { get; set; }
        public string CategoryName { get; set; }
        public Meme Meme { get; set; }
    }
}
