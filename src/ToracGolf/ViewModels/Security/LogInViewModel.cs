using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ToracGolf.ViewModels.Security
{
    public class LogInViewModel
    {

        public LogInViewModel(IList<Navigation.BreadcrumbNavItem> breadcrumb)
        {
            Breadcrumb = breadcrumb;
        }

        public IList<Navigation.BreadcrumbNavItem> Breadcrumb { get;  }
    }
}
