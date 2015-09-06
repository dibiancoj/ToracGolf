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

        public SignUpInViewModel(IList<Navigation.BreadcrumbNavItem> breadcrumb, IImmutableDictionary<string, string> stateListing)
        {
            Breadcrumb = breadcrumb;
            StateListing = stateListing.OrderBy(x => x.Value).Select(x => new SelectListItem { Value = x.Key, Text = x.Value }).ToArray();
            SelectedState = string.Empty;
        }

        public IList<Navigation.BreadcrumbNavItem> Breadcrumb { get; }

        public string SelectedState { get; }
        public IEnumerable<SelectListItem> StateListing { get; }
    }

}
