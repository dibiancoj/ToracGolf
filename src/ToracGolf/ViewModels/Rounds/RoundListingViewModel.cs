using Microsoft.AspNet.Antiforgery;
using Microsoft.AspNet.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToracGolf.MiddleLayer.EFModel.Tables;
using ToracGolf.MiddleLayer.GridCommon;
using ToracGolf.ViewModels.Handicap;

namespace ToracGolf.ViewModels.Rounds
{

    public class RoundListingViewModel
    {

        public RoundListingViewModel(HandicapStatusViewModel handicapStatus,
                                     IList<Navigation.BreadcrumbNavItem> breadcrumb,
                                     //IEnumerable<SelectListItem> stateListing,
                                     AntiforgeryTokenSet tokenSet,
                                     //string usersDefaultState,
                                     int totalNumberofPages,
                                     int totalNumberOfRounds,
                                     IList<SortOrderViewModel> sortOrder,
                                     int defaultRoundsPerPage,
                                     IEnumerable<int> roundsPerPage,
                                     IEnumerable<SelectListItem> userSeasons)
        {
            HandicapStatus = handicapStatus;
            Breadcrumb = breadcrumb;
            //StateListing = stateListing;
            TokenSet = tokenSet;
            //UsersDefaultState = usersDefaultState;
            TotalNumberofPages = totalNumberofPages;
            SortOrder = sortOrder;
            DefaultRoundsPerPage = defaultRoundsPerPage;
            RoundsPerPage = roundsPerPage;
            UserSeasons = userSeasons;
            TotalNumberOfRounds = totalNumberOfRounds;
        }

        public HandicapStatusViewModel HandicapStatus { get; }

        public IList<Navigation.BreadcrumbNavItem> Breadcrumb { get; }

        //public IEnumerable<SelectListItem> StateListing { get; }

        public AntiforgeryTokenSet TokenSet { get; }

       // public string UsersDefaultState { get; }

        public int TotalNumberofPages { get; }

        public int TotalNumberOfRounds { get; }

        public IList<SortOrderViewModel> SortOrder { get; }

        public int DefaultRoundsPerPage { get; }

        public IEnumerable<int> RoundsPerPage { get; }

        public IEnumerable<SelectListItem> UserSeasons { get; }

    }

}
