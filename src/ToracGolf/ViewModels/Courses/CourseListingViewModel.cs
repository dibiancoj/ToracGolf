using Microsoft.AspNet.Antiforgery;
using Microsoft.AspNet.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ToracGolf.ViewModels.Courses
{
    public class CourseListingViewModel
    {

        public CourseListingViewModel(IList<Navigation.BreadcrumbNavItem> breadcrumb, AntiforgeryTokenSet tokenSet, int totalPagesOfCourses)
        {
            Breadcrumb = breadcrumb;
            TokenSet = tokenSet;
            TotalPagesOfCourses = totalPagesOfCourses;
        }

        public IList<Navigation.BreadcrumbNavItem> Breadcrumb { get; }

        public AntiforgeryTokenSet TokenSet { get; }

        public int TotalPagesOfCourses { get; }

    }

}
