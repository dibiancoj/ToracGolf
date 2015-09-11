using Microsoft.AspNet.Authorization;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Mvc.Rendering;
using Microsoft.Framework.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ToracGolf.Constants;
using ToracGolf.MiddleLayer.EFModel;
using ToracGolf.MiddleLayer.SecurityManager;
using ToracGolf.ViewModels.Navigation;
using ToracGolf.ViewModels.Security;
using ToracLibrary.AspNet.Caching.FactoryStore;

namespace ToracGolf.Controllers
{

    [Authorize]
    public class SecurityController : BaseController
    {

        #region Constructor

        public SecurityController(IMemoryCache cache, ICacheFactoryStore cacheFactoryStore, ToracGolfContext dbContext)
        {
            DbContext = dbContext;
            Cache = cache;
            CacheFactory = cacheFactoryStore;
        }

        #endregion

        #region Properties

        private ToracGolfContext DbContext { get; }

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
            if (IsUserAuthenticated())
            {
                //go send them to the main page
                return RedirectToAction("Index", "Home");
            }

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
                //let's try to log this user in
                var userLogInAttempt = await Security.UserLogIn(DbContext, model.Email, model.Password);

                //did we find a user?
                if (userLogInAttempt == null)
                {
                    //can't find the user, add an error
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                }
                else
                {
                    var claims = new List<Claim>();

                    claims.Add(new Claim(ClaimTypes.Name, "Jason DiBianco"));
                    claims.Add(new Claim(ClaimTypes.Email, "dibiancoj@gmail.com"));

                    //sign the user in
                    await Context.Authentication.SignInAsync(SecuritySettings.SecurityType, new ClaimsPrincipal(new ClaimsIdentity(claims, SecuritySettings.SecurityType)));

                    //go send them to the main page
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

            return View(new SignUpInViewModel(breadCrumb, CacheFactory.GetCacheItem<IEnumerable<SelectListItem>>(CacheKeyNames.StateListing, Cache)));
        }

        #endregion

    }
}
