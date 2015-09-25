using Microsoft.AspNet.Antiforgery;
using Microsoft.AspNet.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using ToracGolf.MiddleLayer.Courses;

namespace ToracGolf.ViewModels.Courses
{
    public class CourseAddViewModel
    {

        public CourseAddViewModel(IList<Navigation.BreadcrumbNavItem> breadcrumb, IEnumerable<SelectListItem> stateListing, CourseAddEnteredData courseAddUserEntered, AntiforgeryTokenSet tokenSet)
        {
            CourseAddUserEntered = courseAddUserEntered;
            Breadcrumb = breadcrumb;
            StateListing = stateListing;
            TokenSet = tokenSet;
        }

        public CourseAddEnteredData CourseAddUserEntered { get; set; }

        public IList<Navigation.BreadcrumbNavItem> Breadcrumb { get; set; }

        public IEnumerable<SelectListItem> StateListing { get; }

        public AntiforgeryTokenSet TokenSet { get; }

    }

}
