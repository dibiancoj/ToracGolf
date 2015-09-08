using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using ToracGolf.ViewModels.Home;
using ToracGolf.ViewModels.Navigation;

namespace ToracGolf.Controllers
{
    public class HomeController : Controller
    {

        //[Route("Home", Name = "Home")]
        [HttpGet]
        public IActionResult Index()
        {
           return RedirectToAction("LogIn", "Security", new { });

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
