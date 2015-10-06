using Microsoft.AspNet.Antiforgery;
using Microsoft.AspNet.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToracGolf.MiddleLayer.Rounds;

namespace ToracGolf.ViewModels.Rounds
{

    public class RoundAddViewModel
    {

        public RoundAddViewModel(IList<Navigation.BreadcrumbNavItem> breadcrumb, IEnumerable<SelectListItem> stateListing, AntiforgeryTokenSet tokenSet, RoundAddEnteredData roundEnteredData)
        {
            Breadcrumb = breadcrumb;
            StateListing = stateListing;
            TokenSet = tokenSet;
            RoundEnteredData = roundEnteredData;
        }

        public IList<Navigation.BreadcrumbNavItem> Breadcrumb { get; set; }

        public IEnumerable<SelectListItem> StateListing { get; }

        public AntiforgeryTokenSet TokenSet { get; }

        public RoundAddEnteredData RoundEnteredData { get; }

    }

}
