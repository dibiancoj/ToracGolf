using Microsoft.AspNet.Antiforgery;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToracGolf.MiddleLayer.Dashboard.Models;
using ToracGolf.ViewModels.Handicap;

namespace ToracGolf.ViewModels.Home
{
    public class IndexViewModel
    {
        public IndexViewModel(HandicapStatusViewModel handicapStatus,
                              IList<Navigation.BreadcrumbNavItem> breadcrumb,
                              AntiforgeryTokenSet tokenSet,
                              DashboardViewType.DashboardViewTypeEnum viewType)
        {
            HandicapStatus = handicapStatus;
            Breadcrumb = breadcrumb;
            TokenSet = tokenSet;
            ViewType = viewType;
        }

        public HandicapStatusViewModel HandicapStatus { get; }

        public IList<Navigation.BreadcrumbNavItem> Breadcrumb { get; set; }

        public AntiforgeryTokenSet TokenSet { get; }

        public DashboardViewType.DashboardViewTypeEnum ViewType { get; set; }

    }
}
