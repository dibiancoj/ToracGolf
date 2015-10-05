using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ToracGolf.MiddleLayer.EFModel.Tables
{
    public class CourseImages
    {

        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Key()]
        public int CourseId { get; set; }

        //public byte[] CourseImage { get; set; }
        public byte[] CourseImage { get; set; }

    }
}
