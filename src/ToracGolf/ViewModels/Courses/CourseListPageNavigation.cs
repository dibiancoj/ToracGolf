using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using static ToracGolf.MiddleLayer.Courses.CourseListingSortOrder;

namespace ToracGolf.ViewModels.Courses
{

    /// <summary>
    /// couldn't get serialization to work when i pass in an int to the controller (when they change the page). just going to use an object
    /// </summary>
    public class CourseListPageNavigation
    {

        #region Properties

        public bool ResetPager { get; set; }

        public int PageIndexId { get; set; }

        public CourseListingSortEnum SortBy { get; set; }

        public string CourseNameFilter { get; set; }

        public string StateFilter { get; set; }

        public int CoursesPerPage { get; set; }

        #endregion

    }

}
