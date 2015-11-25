using Microsoft.AspNet.Antiforgery;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToracGolf.MiddleLayer.EFModel.Tables;
using ToracGolf.ViewModels.Handicap;

namespace ToracGolf.ViewModels.Courses
{
    public class CourseStatsViewModel
    {

        public CourseStatsViewModel(HandicapStatusViewModel handicapStatus,
                                    IList<Navigation.BreadcrumbNavItem> breadcrumb,
                                    AntiforgeryTokenSet tokenSet,
                                    Course course)
        {
            HandicapStatus = handicapStatus;
            Breadcrumb = breadcrumb;
            TokenSet = tokenSet;
            CourseRecord = course;
        }

        public HandicapStatusViewModel HandicapStatus { get; }

        public IList<Navigation.BreadcrumbNavItem> Breadcrumb { get; }

        public AntiforgeryTokenSet TokenSet { get; }

        public Course CourseRecord;

    }

}
