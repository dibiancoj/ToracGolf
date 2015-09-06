using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ToracGolf.ViewModels.Home
{
    public class IndexViewModel
    {
        public IndexViewModel(IList<Navigation.BreadcrumbNavItem> breadcrumb)
        {
            Breadcrumb = breadcrumb;
        }

        public IList<Navigation.BreadcrumbNavItem> Breadcrumb { get; }
    }
}
