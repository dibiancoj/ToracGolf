using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ToracGolf.ViewModels.Security
{
    public class SignUpInViewModel
    {

        public SignUpInViewModel(IList<Navigation.BreadcrumbNavItem> breadcrumb)
        {
            Breadcrumb = breadcrumb;
        }

        public IList<Navigation.BreadcrumbNavItem> Breadcrumb { get; }
    }

}
