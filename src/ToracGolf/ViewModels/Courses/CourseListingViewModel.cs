﻿using Microsoft.AspNet.Antiforgery;
using Microsoft.AspNet.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToracGolf.MiddleLayer.Courses;

namespace ToracGolf.ViewModels.Courses
{
    public class CourseListingViewModel
    {

        public CourseListingViewModel(IList<Navigation.BreadcrumbNavItem> breadcrumb, AntiforgeryTokenSet tokenSet, int totalPagesOfCourses, IEnumerable<CourseListingSortOrderModel> sortOrder)
        {
            Breadcrumb = breadcrumb;
            TokenSet = tokenSet;
            TotalPagesOfCourses = totalPagesOfCourses;
            SortOrder = sortOrder;
        }

        public IList<Navigation.BreadcrumbNavItem> Breadcrumb { get; }

        public AntiforgeryTokenSet TokenSet { get; }

        public int TotalPagesOfCourses { get; }

        public IEnumerable<CourseListingSortOrderModel> SortOrder { get; }

    }

}
