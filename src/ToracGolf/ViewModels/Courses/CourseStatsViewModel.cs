using Microsoft.AspNet.Antiforgery;
using Microsoft.AspNet.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToracGolf.MiddleLayer.Courses.Models;
using ToracGolf.ViewModels.Handicap;

namespace ToracGolf.ViewModels.Courses
{
    public class CourseStatsViewModel
    {

        public CourseStatsViewModel(HandicapStatusViewModel handicapStatus,
                                    IList<Navigation.BreadcrumbNavItem> breadcrumb,
                                    AntiforgeryTokenSet tokenSet,
                                    CourseStatsModel course,
                                    IEnumerable<SelectListItem> userSeasons,
                                    IEnumerable<SelectListItem> teeBoxLocations)
        {
            HandicapStatus = handicapStatus;
            Breadcrumb = breadcrumb;
            TokenSet = tokenSet;
            CourseRecord = course;
            UserSeasons = userSeasons;
            TeeBoxLocations = teeBoxLocations;
        }

        public HandicapStatusViewModel HandicapStatus { get; }

        public IList<Navigation.BreadcrumbNavItem> Breadcrumb { get; }

        public AntiforgeryTokenSet TokenSet { get; }

        public CourseStatsModel CourseRecord { get; }

        public IEnumerable<SelectListItem> UserSeasons { get; }

        public IEnumerable<SelectListItem> TeeBoxLocations { get; }

    }

}
