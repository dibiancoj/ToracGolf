using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc.Rendering;

namespace ToracGolf.ViewModels.Security
{
    public class SignUpInViewModel
    {

        public SignUpInViewModel(IList<Navigation.BreadcrumbNavItem> breadcrumb, IEnumerable<SelectListItem> stateListing)
        {
            Breadcrumb = breadcrumb;
            StateListing = stateListing;
            SelectedState = string.Empty;
        }

        public IList<Navigation.BreadcrumbNavItem> Breadcrumb { get; }

        public string SelectedState { get; }

        public IEnumerable<SelectListItem> StateListing { get; }
    }

}
