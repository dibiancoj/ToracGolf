using Microsoft.AspNet.Antiforgery;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToracGolf.ViewModels.Handicap;

namespace ToracGolf.ViewModels.Settings
{

    public class AddAndEditFriendsViewModel
    {

        public AddAndEditFriendsViewModel(HandicapStatusViewModel handicapStatus, IList<Navigation.BreadcrumbNavItem> breadcrumb, AntiforgeryTokenSet tokenSet)
        {
            HandicapStatus = handicapStatus;
            Breadcrumb = breadcrumb;
            TokenSet = tokenSet;
        }

        public HandicapStatusViewModel HandicapStatus { get; }

        public IList<Navigation.BreadcrumbNavItem> Breadcrumb { get; set; }

        public AntiforgeryTokenSet TokenSet { get; }

    }

}
