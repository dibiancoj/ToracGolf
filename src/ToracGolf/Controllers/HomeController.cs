using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using ToracGolf.ViewModels.Home;
using ToracGolf.ViewModels.Navigation;
using Microsoft.AspNet.Authorization;

namespace ToracGolf.Controllers
{

    [Authorize]
    public class HomeController : BaseController
    {

        //[Route("Home", Name = "Home")]
        [HttpGet]
        public IActionResult Index()
        {
            var breadCrumb = new List<BreadcrumbNavItem>();

            breadCrumb.Add(new BreadcrumbNavItem("Home", "#"));

            return View("Index", new IndexViewModel(breadCrumb));
        }

        public IActionResult Error()
        {
            return View("~/Views/Shared/Error.cshtml");
        }
    }

}
