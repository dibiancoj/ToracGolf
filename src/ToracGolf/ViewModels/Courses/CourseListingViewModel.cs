using Microsoft.AspNet.Antiforgery;
using Microsoft.AspNet.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ToracGolf.ViewModels.Courses
{
    public class CourseListingViewModel
    {

        public CourseListingViewModel(IList<Navigation.BreadcrumbNavItem> breadcrumb, AntiforgeryTokenSet tokenSet)
        {
            Breadcrumb = breadcrumb;
            TokenSet = tokenSet;
        }

        public IList<Navigation.BreadcrumbNavItem> Breadcrumb { get; set; }

        public AntiforgeryTokenSet TokenSet { get; }

    }

}
