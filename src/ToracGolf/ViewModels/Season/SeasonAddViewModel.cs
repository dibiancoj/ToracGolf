using Microsoft.AspNet.Antiforgery;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToracGolf.ViewModels.Handicap;
using ToracGolf.MiddleLayer.Season;

namespace ToracGolf.ViewModels.Season
{

    public class SeasonAddViewModel
    {

        public SeasonAddViewModel(HandicapStatusViewModel handicapStatus, IList<Navigation.BreadcrumbNavItem> breadcrumb, AntiforgeryTokenSet tokenSet, SeasonAddEnteredData seasonAddEnteredData)
        {
            HandicapStatus = handicapStatus;      
            Breadcrumb = breadcrumb;
            TokenSet = tokenSet;
            EnteredData = seasonAddEnteredData;
        }

        public HandicapStatusViewModel HandicapStatus { get; }

        public IList<Navigation.BreadcrumbNavItem> Breadcrumb { get; set; }

        public AntiforgeryTokenSet TokenSet { get; }

        public SeasonAddEnteredData EnteredData { get; }

    }

}
