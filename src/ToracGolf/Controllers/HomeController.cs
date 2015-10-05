using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using ToracGolf.ViewModels.Home;
using ToracGolf.ViewModels.Navigation;
using ToracGolf.MiddleLayer.EFModel.Tables;
using System.Security.Claims;
using ToracGolf.Constants;

namespace ToracGolf.Controllers
{

    //[Authorize]
    public class HomeController : BaseController
    {

        //[Route("Home", Name = "Home")]
        [HttpGet]
        public IActionResult Index()
        {
            //let's try to log this user in
            var userLogInAttempt = (new UserAccounts { UserId = 21, EmailAddress = "dibiancoj@gmail.com", Password = "0", StateId = 32 });// await Security.UserLogIn(DbContext, model.Email, model.Password);

            var claims = new List<Claim>();

            claims.Add(new Claim(ClaimTypes.Hash, userLogInAttempt.UserId.ToString()));
            claims.Add(new Claim(ClaimTypes.Name, userLogInAttempt.FirstName + " " + userLogInAttempt.LastName));
            claims.Add(new Claim(ClaimTypes.Email, userLogInAttempt.EmailAddress));
            claims.Add(new Claim(ClaimTypes.StateOrProvince, userLogInAttempt.StateId.ToString()));

            //sign the user in
            Context.Authentication.SignInAsync(SecuritySettings.SecurityType, new ClaimsPrincipal(new ClaimsIdentity(claims, SecuritySettings.SecurityType))).Wait();

            return RedirectToRoute("AddACourse");











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
