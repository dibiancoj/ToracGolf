using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using ToracGolf.ViewModels.Navigation;
using ToracGolf.ViewModels.Security;
using Microsoft.AspNet.Authorization;
using ToracGolf.Models;
using Microsoft.AspNet.Identity;
using System.Collections.Immutable;
using Microsoft.Framework.Caching.Memory;
using Microsoft.AspNet.Mvc.Rendering;
using ToracLibrary.AspNet.Caching.FactoryStore;
using ToracGolf.Constants;

namespace ToracGolf.Controllers
{

    [Authorize]
    public class SecurityController : Controller
    {

        #region Constructor

        public SecurityController(SignInManager<ApplicationUser> signInManagerAPI, IMemoryCache cache, ICacheFactoryStore cacheFactoryStore)
        {
            SignInManagerAPI = signInManagerAPI;
            Cache = cache;
            CacheFactory = cacheFactoryStore;
        }

        #endregion

        #region Properties

        private SignInManager<ApplicationUser> SignInManagerAPI { get; }

        private IMemoryCache Cache { get; }

        private ICacheFactoryStore CacheFactory { get; }

        #endregion

        #region Methods

        #region Log In

        private static IList<BreadcrumbNavItem> BuildLogInBreadcrumb()
        {
            var breadCrumb = new List<BreadcrumbNavItem>();

            breadCrumb.Add(new BreadcrumbNavItem("Home", "#"));
            breadCrumb.Add(new BreadcrumbNavItem("Log In", "#"));

            return breadCrumb;
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("LogIn")]
        public IActionResult LogIn()
        {
            return View(new LogInViewModel { Breadcrumb = BuildLogInBreadcrumb(), LogInUserEntered = new LogInEnteredData() });
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LogInAttempt(LogInEnteredData model)
        {
            if (ModelState.IsValid)
            {
                // This doesn't count login failures towards account lockout
                // To enable password failures to trigger account lockout, set lockoutOnFailure: true
                // var result = await SignInManagerAPI.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);
                //if (result.Succeeded)
                //{
                //    return RedirectToLocal(returnUrl);
                //}
                //if (result.RequiresTwoFactor)
                //{
                //    return RedirectToAction(nameof(SendCode), new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
                //}
                //if (result.IsLockedOut)
                //{
                //    return View("Lockout");
                //}
                // else
                // {
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                // return View("LogIn", model);
                // }
            }

            ModelState.AddModelError(string.Empty, "Invalid login attempt.");

            // If we got this far, something failed, redisplay form
            return View("LogIn", new LogInViewModel { Breadcrumb = BuildLogInBreadcrumb(), LogInUserEntered = model });
        }

        #endregion

        #endregion

        #region Sign Up

        [HttpGet]
        [AllowAnonymous]
        [Route("SignUp")]
        public IActionResult SignUp()
        {
            var breadCrumb = new List<BreadcrumbNavItem>();

            breadCrumb.Add(new BreadcrumbNavItem("Home", "#"));
            breadCrumb.Add(new BreadcrumbNavItem("Sign Up", "#"));

            return View(new SignUpInViewModel(breadCrumb, CacheFactory.GetCacheItem<IEnumerable<SelectListItem>>(CacheKeyNames.StateListing, Cache)));
        }

        #endregion

    }
}
