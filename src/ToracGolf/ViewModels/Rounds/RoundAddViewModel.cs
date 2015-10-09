using Microsoft.AspNet.Antiforgery;
using Microsoft.AspNet.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToracGolf.MiddleLayer.Rounds;
using ToracGolf.ViewModels.Handicap;

namespace ToracGolf.ViewModels.Rounds
{

    public class RoundAddViewModel
    {

        public RoundAddViewModel(HandicapStatusViewModel handicapStatus, IList<Navigation.BreadcrumbNavItem> breadcrumb, IEnumerable<SelectListItem> stateListing, AntiforgeryTokenSet tokenSet, RoundAddEnteredData roundEnteredData)
        {
            HandicapStatus = handicapStatus;
            Breadcrumb = breadcrumb;
            StateListing = stateListing;
            TokenSet = tokenSet;
            RoundEnteredData = roundEnteredData;
        }

        public HandicapStatusViewModel HandicapStatus { get; }

        public IList<Navigation.BreadcrumbNavItem> Breadcrumb { get; set; }

        public IEnumerable<SelectListItem> StateListing { get; }

        public AntiforgeryTokenSet TokenSet { get; }

        public RoundAddEnteredData RoundEnteredData { get; }

    }

}
