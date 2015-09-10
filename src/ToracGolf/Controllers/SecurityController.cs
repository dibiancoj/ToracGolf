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
using System.Security.Claims;
using ToracGolf.Settings;

namespace ToracGolf.Controllers
{

    [Authorize]
    public class SecurityController : Controller
    {

        #region Constructor

        public SecurityController(IMemoryCache cache, ICacheFactoryStore cacheFactoryStore , AppSettings settings)
        {
            AppSetting = settings;
            Cache = cache;
            CacheFactory = cacheFactoryStore;
        }

        #endregion

        #region Properties

        private AppSettings AppSetting { get; }

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
        [Route("LogIn", Name = "LogIn")]
        public IActionResult LogIn()
        {
            return View(new LogInViewModel { Breadcrumb = BuildLogInBreadcrumb(), LogInUserEntered = new LogInEnteredData() });
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("LogIn", Name = "LogIn")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LogIn(LogInEnteredData model)
        {
            //do we have a valid model?
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
                // return View("LogIn", model);
                // }


                var claims = new List<Claim>();
                claims.Add(new Claim(ClaimTypes.Name, "Jason DiBianco"));
                claims.Add(new Claim(ClaimTypes.Email, "dibiancoj@gmail.com"));
                var id = new ClaimsIdentity(claims, "Cookies");
                await Context.Authentication.SignInAsync("Cookies", new ClaimsPrincipal(id));

                //replace with database call
                if (string.Equals(model.Email, "dibiancoj@gmail.com", StringComparison.OrdinalIgnoreCase))
                {
                    //var result = await SignInManagerAPI.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);

                    return RedirectToAction("Index", "Home");
                }
            }

            // If we got this far, something failed, redisplay form
            return View("LogIn", new LogInViewModel { Breadcrumb = BuildLogInBreadcrumb(), LogInUserEntered = model });
        }

        #endregion

        #endregion

        #region Sign Up

        [HttpGet]
        [AllowAnonymous]
        [Route("SignUp", Name = "SignUp")]
        public IActionResult SignUp()
        {
            var breadCrumb = new List<BreadcrumbNavItem>();

            breadCrumb.Add(new BreadcrumbNavItem("Home", "#"));
            breadCrumb.Add(new BreadcrumbNavItem("Sign Up", "#"));

            var t = MiddleLayer.States.StateListing.StateSelect(AppSetting.ConnectionString);

            return View(new SignUpInViewModel(breadCrumb, CacheFactory.GetCacheItem<IEnumerable<SelectListItem>>(CacheKeyNames.StateListing, Cache)));
        }

        #endregion

    }
}
