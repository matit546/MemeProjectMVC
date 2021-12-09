using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MemesProject.Models
{
    public class Comment
    {
        public long IdComment { get; set; }
        public string Text { get; set; }
        public DateTime Date { get; set; }
        public bool IfBlocked { get; set; }
        public int Likes { get; set; }
        public int Dislikes { get; set; }
        public long IdCommentsHub { get; set; }
        //public CommentsHub CommentsHub { get; set; }
        public string IdUser { get; set; }
        //public CommentActivity CommentActivity { get; set; }
    }
}
