using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Authorization;
using ToracGolf.ViewModels.Navigation;
using ToracGolf.ViewModels.Courses;

namespace ToracGolf.Controllers
{

    [Authorize]
    public class CourseController : BaseController
    {

        #region Add A Course


        private static IList<BreadcrumbNavItem> BuildAddACourseBreadcrumb()
        {
            var breadCrumb = new List<BreadcrumbNavItem>();

            breadCrumb.Add(new BreadcrumbNavItem("Home", "/"));
            breadCrumb.Add(new BreadcrumbNavItem("Courses", "#"));
            breadCrumb.Add(new BreadcrumbNavItem("Add A Course", "#"));

            return breadCrumb;
        }

        [HttpGet]
        [Route("AddACourse", Name = "AddACourse")]
        public IActionResult CourseAdd()
        {
            return View(new CourseAddViewModel
            {
                Breadcrumb = BuildAddACourseBreadcrumb()
            });
        }

        #endregion

    }
}
