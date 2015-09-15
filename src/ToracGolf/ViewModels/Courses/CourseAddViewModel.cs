using Microsoft.AspNet.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ToracGolf.ViewModels.Courses
{
    public class CourseAddViewModel
    {

        public CourseAddViewModel(IList<Navigation.BreadcrumbNavItem> breadcrumb, IEnumerable<SelectListItem> stateListing, CourseAddEnteredData courseAddUserEntered)
        {
            CourseAddUserEntered = courseAddUserEntered;
            Breadcrumb = breadcrumb;
            StateListing = stateListing;
        }

        public CourseAddEnteredData CourseAddUserEntered { get; set; }

        public IList<Navigation.BreadcrumbNavItem> Breadcrumb { get; set; }

        public IEnumerable<SelectListItem> StateListing { get; }
    }

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

        #endregion

    }
}
