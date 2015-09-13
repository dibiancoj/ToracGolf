using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ToracGolf.ViewModels.Courses
{
    public class CourseAddViewModel
    {
        public CourseAddEnteredData CourseAddUserEntered { get; set; }

        public IList<Navigation.BreadcrumbNavItem> Breadcrumb { get; set; }
    }

    public class CourseAddEnteredData
    {

        #region Model Properties

        //[Required]
        //[EmailAddress]
        //public string Email { get; set; }

        //[Required]
        //[DataType(DataType.Password)]
        //public string Password { get; set; }

        //[Display(Name = "Remember me?")]
        //public bool RememberMe { get; set; }

        #endregion

    }
}
