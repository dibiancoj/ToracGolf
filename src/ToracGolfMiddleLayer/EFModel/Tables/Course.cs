using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ToracGolf.MiddleLayer.EFModel.Tables
{
    public class Course
    {

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CourseId { get; set; }

        [Required]
        [StringLength(75)]
        public string Name { get; set; }

        [Required]
        [StringLength(100)]
        public string City { get; set; }

        [Required]
        public int StateId { get; set; }

        [Required]
        [StringLength(200)]
        public string Description { get; set; }

        [Required]
        public int UserIdThatCreatedCourse { get; set; }

        [Required]
        public bool Pending { get; set; }

        [Required]
        public bool IsActive { get; set; }

        [Required]
        public bool OnlyAllow18Holes { get; set; }

        public ICollection<CourseTeeLocations> CourseTeeLocations { get; set; }

        //public CourseImages CourseImage { get; set; }

        public Ref_State State { get; set; }

    }
}
