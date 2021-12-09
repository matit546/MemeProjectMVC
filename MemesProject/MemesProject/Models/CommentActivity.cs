using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MemesProject.Models
{
    public class CommentActivity
    {
        public long IdCommentActivity { get; set; }
        public long IdComment { get; set; }
        //public Comment Comment { get; set; }
        public string IdUser { get; set; }
    }
}
