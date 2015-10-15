using Microsoft.AspNet.Antiforgery;
using Microsoft.AspNet.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToracGolf.ViewModels.Handicap;

namespace ToracGolf.ViewModels.Season
{
    public class SeasonListingViewModel
    {

        public SeasonListingViewModel(HandicapStatusViewModel handicapStatus,
                                      IList<Navigation.BreadcrumbNavItem> breadcrumb,
                                      AntiforgeryTokenSet tokenSet)
        {
            HandicapStatus = handicapStatus;
            Breadcrumb = breadcrumb;
            TokenSet = tokenSet;
        }

        public HandicapStatusViewModel HandicapStatus { get; }

        public IList<Navigation.BreadcrumbNavItem> Breadcrumb { get; }

        public AntiforgeryTokenSet TokenSet { get; }

    }

}
