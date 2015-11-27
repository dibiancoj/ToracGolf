using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ToracGolf.MiddleLayer.EFModel.Tables
{
    public class Ref_State
    {

        [Key]
        public int StateId { get; set; }

        [Required]
        [StringLength(100)]
        public string Description { get; set; }

        public ICollection<Course> Courses { get; set; }

    }
}
