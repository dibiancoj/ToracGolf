using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using ToracGolf.ViewModels.Navigation;
using ToracGolf.ViewModels.Security;

namespace ToracGolf.Controllers
{
    public class SecurityController : Controller
    {

        [Route("LogIn")]
        public IActionResult LogIn()
        {
            var breadCrumb = new List<BreadcrumbNavItem>();

            breadCrumb.Add(new BreadcrumbNavItem("Home", "#"));
            breadCrumb.Add(new BreadcrumbNavItem("LogIn", "#"));

            return View(new LogInViewModel(breadCrumb));
        }

    }
}
