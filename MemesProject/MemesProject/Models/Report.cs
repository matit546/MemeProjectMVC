using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MemesProject.Models
{
    public class Report
    {
        public long IdReport { get; set; }
        public long IdMeme { get; set; }
        public DateTime Date { get; set; }
        public string IdUser { get; set; }
    }
}
