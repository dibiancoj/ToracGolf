using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc.Rendering;
using ToracGolf.MiddleLayer.Security;

namespace ToracGolf.ViewModels.Security
{
    public class SignUpInViewModel
    {

        public SignUpInViewModel(IList<Navigation.BreadcrumbNavItem> breadcrumb, IEnumerable<SelectListItem> stateListing, SignUpEnteredData signupModel)
        {
            Breadcrumb = breadcrumb;
            StateListing = stateListing;
            SelectedState = string.Empty;
            SignUpUserEntered = signupModel;
        }

        public IList<Navigation.BreadcrumbNavItem> Breadcrumb { get; }

        public string SelectedState { get; }

        public IEnumerable<SelectListItem> StateListing { get; }

        public SignUpEnteredData SignUpUserEntered { get; set; }
    }

}
