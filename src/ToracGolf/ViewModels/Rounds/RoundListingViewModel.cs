using Microsoft.AspNet.Antiforgery;
using Microsoft.AspNet.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToracGolf.ViewModels.Handicap;

namespace ToracGolf.ViewModels.Rounds
{

    public class RoundListingViewModel
    {

        public RoundListingViewModel(HandicapStatusViewModel handicapStatus, IList<Navigation.BreadcrumbNavItem> breadcrumb, IEnumerable<SelectListItem> stateListing, AntiforgeryTokenSet tokenSet, string usersDefaultState)
        {
            HandicapStatus = handicapStatus;
            Breadcrumb = breadcrumb;
            StateListing = stateListing;
            TokenSet = tokenSet;
            UsersDefaultState = usersDefaultState;  
        }


        public HandicapStatusViewModel HandicapStatus { get; }

        public IList<Navigation.BreadcrumbNavItem> Breadcrumb { get; set; }

        public IEnumerable<SelectListItem> StateListing { get; }

        public AntiforgeryTokenSet TokenSet { get; }

        public string UsersDefaultState { get; }

    }

}
