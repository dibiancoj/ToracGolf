using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using ToracGolf.MiddleLayer.CustomValidations;

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

        [Display(Name = "Only Allow 18 Holes")]
        [Required]
        public bool OnlyAllow18Holes { get; set; }

        /// <summary>
        /// This will be base 64 encoded from the json passed in
        /// </summary>
        public string CourseImage { get; set; }

        [EnsureMinimumElements(1, ErrorMessage = "No Tee Locations Have Been Entered. You Need Atleast 1 Tee Location.")]
        [Display(Name = "Tee Box Locations")]
        public IList<CourseAddEnteredDataTeeLocations> TeeLocations { get; set; }

        #endregion

    }

    public class CourseAddEnteredDataTeeLocations
    {

        [Display(Name = "Description")]
        [Required]
        [StringLength(50)]
        public string Description { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Yardage Must Be Greater Than 0")]
        [Required]
        public int Yardage { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Front 9 Par Must Be Greater Than 0")]
        [Required]
        public int Front9Par { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Back 9 Par Must Be Greater Than 0")]
        [Required]
        public int Back9Par { get; set; }

        [Range(50, 100, ErrorMessage = "Rating Must Be Between 50 and 100")]
        [Required]
        public decimal Rating { get; set; }

        [Range(55, 155, ErrorMessage = "Slope Must Be Between 55 and 155")]
        [Required]
        public decimal Slope { get; set; }

    }

}
