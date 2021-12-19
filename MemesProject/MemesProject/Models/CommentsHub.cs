using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MemesProject.Models
{
    public class CommentsHub
    {
        [Key]
        public long IdCommentHub { get; set; }
        //public long IdComment { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
        public long IdMeme { get; set; }
        public virtual Meme Meme { get; set; }
    }
}
