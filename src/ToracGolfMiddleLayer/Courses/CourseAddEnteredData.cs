using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ToracGolf.MiddleLayer.Courses
{
    public class CourseAddEnteredData
    {

        #region Model Properties

        [Display(Name = "Course Name")]
        [Required]
        [MaxLength(75)]
        public string CourseName { get; set; }

        [Display(Name = "Location")]
        [Required]
        [MaxLength(100)]
        public string Location { get; set; }

        [Display(Name = "State")]
        [Required]
        public string StateListing { get; set; }

        [Display(Name = "Description")]
        [Required]
        [MaxLength(200)]
        public string Description { get; set; }

        #endregion

    }
}
