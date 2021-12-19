using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MemesProject.Models
{
    public class Comment
    {
        [Key]
        public long IdComment { get; set; }
        public string Text { get; set; }
        public DateTime Date { get; set; }
        public bool IfBlocked { get; set; }
        public int Likes { get; set; }
        public int Dislikes { get; set; }
        public long IdCommentsHub { get; set; }
        public virtual CommentsHub CommentsHub { get; set; }
        public string IdUser { get; set; }
        //public virtual CommentActivity CommentActivity { get; set; }
    }
}
