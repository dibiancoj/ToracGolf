﻿using System;
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
        [Required(ErrorMessage = "Course Name Is A Required Field")]
        [MaxLength(75)]
        public string CourseName { get; set; }

        [Display(Name = "City")]
        [Required(ErrorMessage = "City Is A Required Field")]
        [MaxLength(100)]
        public string City { get; set; }

        [Display(Name = "State")]
        [Required(ErrorMessage = "State Is A Required Field")]
        public string StateListing { get; set; }

        [Display(Name = "Description")]
        [Required(ErrorMessage = "Description Is A Required Field")]
        [MaxLength(200)]
        public string Description { get; set; }

        [Display(Name = "Only Allow 18 Holes")]
        [Required]
        public bool OnlyAllow18Holes { get; set; }

        /// <summary>
        /// This will be base 64 encoded from the json passed in
        /// </summary>
        public string CourseImage { get; set; }

        [EnsureMinimumElements(1, ErrorMessage = "No Tee Locations Have Been Entered")]
        [Display(Name = "Tee Box Locations")]
        public IList<CourseAddEnteredDataTeeLocations> TeeLocations { get; set; }

        #endregion

    }

    public class CourseAddEnteredDataTeeLocations
    {

        [Display(Name = "Description")]
        [Required(ErrorMessage = "Description Is A Required Field")]
        [StringLength(50)]
        public string Description { get; set; }

        [Display(Name = "Yardage")]
        [Range(1, int.MaxValue, ErrorMessage = "Yardage Must Be Greater Than 0")]
        [Required(ErrorMessage = "Yardage Is A Required Field")]
        public int? Yardage { get; set; }

        [Display(Name = "Front 9 Par")]
        [Range(1, int.MaxValue, ErrorMessage = "Front 9 Par Must Be Greater Than 0")]
        [Required(ErrorMessage = "Front 9 Par Is A Required Field")]
        public int? Front9Par { get; set; }

        [Display(Name = "Back 9 Par")]
        [Range(1, int.MaxValue, ErrorMessage = "Back 9 Par Must Be Greater Than 0")]
        [Required(ErrorMessage = "Back 9 Par Is A Required Field")]
        public int? Back9Par { get; set; }

        [Display(Name = "Rating")]
        [Range(50, 100, ErrorMessage = "Rating Must Be Between 50 and 100")]
        [Required(ErrorMessage = "Rating Is A Required Field")]
        public double? Rating { get; set; }

        [Display(Name = "Slope")]
        [Range(55, 155, ErrorMessage = "Slope Must Be Between 55 and 155")]
        [Required(ErrorMessage = "Slope Is A Required Field")]
        public double? Slope { get; set; }

    }

}