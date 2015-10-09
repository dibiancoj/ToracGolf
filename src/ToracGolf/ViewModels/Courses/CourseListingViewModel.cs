using Microsoft.AspNet.Antiforgery;
using Microsoft.AspNet.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToracGolf.MiddleLayer.Courses;
using ToracGolf.MiddleLayer.GridCommon;
using ToracGolf.ViewModels.Handicap;

namespace ToracGolf.ViewModels.Courses
{
    public class CourseListingViewModel
    {

        public CourseListingViewModel(HandicapStatusViewModel handicapStatus,
                                      IList<Navigation.BreadcrumbNavItem> breadcrumb,
                                      AntiforgeryTokenSet tokenSet,
                                      int totalPagesOfCourses,
                                      IEnumerable<SortOrderViewModel> sortOrder,
                                      IEnumerable<SelectListItem> stateListing,
                                      string userStatePreference,
                                      int defaultCoursesPerPage,
                                      IEnumerable<int> coursesPerPage)
        {
            HandicapStatus = handicapStatus;
            Breadcrumb = breadcrumb;
            TokenSet = tokenSet;
            TotalPagesOfCourses = totalPagesOfCourses;
            SortOrder = sortOrder;
            StateListing = stateListing;
            UserStatePreference = userStatePreference;
            DefaultCoursesPerPage = defaultCoursesPerPage;
            CoursesPerPage = coursesPerPage;
        }

        public HandicapStatusViewModel HandicapStatus { get; }

        public IList<Navigation.BreadcrumbNavItem> Breadcrumb { get; }

        public AntiforgeryTokenSet TokenSet { get; }

        public int TotalPagesOfCourses { get; }

        public IEnumerable<SortOrderViewModel> SortOrder { get; }

        public IEnumerable<SelectListItem> StateListing { get; }

        public string UserStatePreference { get; }

        public int DefaultCoursesPerPage { get; }

        public IEnumerable<int> CoursesPerPage { get; }

    }

}
