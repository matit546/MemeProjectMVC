using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MemesProject.Models
{
    public class Observation
    {
        [Key]
        public long IdObservation { get; set; }
        public string IdUser { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        public string IdObservedUser { get; set; }
    }
}
