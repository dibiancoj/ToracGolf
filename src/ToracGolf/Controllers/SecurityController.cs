using Microsoft.AspNet.Authorization;
using Microsoft.AspNet.Http;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Mvc.Rendering;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.OptionsModel;
using Microsoft.Net.Http.Headers;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ToracGolf.Constants;
using ToracGolf.MiddleLayer;
using ToracGolf.MiddleLayer.EFModel;
using ToracGolf.MiddleLayer.EFModel.Tables;
using ToracGolf.MiddleLayer.Security;
using ToracGolf.MiddleLayer.SecurityManager;
using ToracGolf.Settings;
using ToracGolf.ViewModels.Navigation;
using ToracGolf.ViewModels.Security;
using ToracLibrary.AspNet.Caching.FactoryStore;

namespace ToracGolf.Controllers
{

    [Authorize]
    public class SecurityController : BaseController
    {

        #region Constructor

        public SecurityController(IMemoryCache cache, ICacheFactoryStore cacheFactoryStore, ToracGolfContext dbContext, IOptions<AppSettings> configuration)
        {
            DbContext = dbContext;
            Cache = cache;
            CacheFactory = cacheFactoryStore;
            Configuration = configuration;
        }

        #endregion

        #region Properties

        private ToracGolfContext DbContext { get; }

        private IMemoryCache Cache { get; }

        private ICacheFactoryStore CacheFactory { get; }

        private IOptions<AppSettings> Configuration { get; }

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

        private async Task LogUserIn(UserAccounts userLogInAttempt)
        {
            var claims = new List<Claim>();

            claims.Add(new Claim(ClaimTypes.Hash, userLogInAttempt.UserId.ToString()));
            claims.Add(new Claim(ClaimTypes.Name, userLogInAttempt.FirstName + " " + userLogInAttempt.LastName));
            claims.Add(new Claim(ClaimTypes.Email, userLogInAttempt.EmailAddress));
            claims.Add(new Claim(ClaimTypes.StateOrProvince, userLogInAttempt.StateId.ToString()));

            claims.Add(new Claim(SecuritySettings.UserImageClaimUrl, CacheFactory.GetCacheItem<ImageFinder>(CacheKeyNames.UserImageFinder, Cache).FindImage(userLogInAttempt.UserId)));
            //claims.Add(new Claim(SecuritySettings.UserImageClaimUrl, "https://lh3.googleusercontent.com/--LDT0wSuaA8/AAAAAAAAAAI/AAAAAAAAAEU/jIcMb4MHAQE/s120-c/photo.jpg"));


            //sign the user in
            await HttpContext.Authentication.SignInAsync(SecuritySettings.SecurityType, new ClaimsPrincipal(new ClaimsIdentity(claims, SecuritySettings.SecurityType)));
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("LogIn", Name = "LogIn")]
        //don't cache it so when they hit the back button, they won't get an anti forgery message. so they log in, and they can't get back without logging out
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult LogIn()
        {
            #region Only Here For Debugging
#if DEBUG
            LogUserIn(SecurityDataProvider.UserLogIn(DbContext, "dibiancoj@gmail.com", "0").Result).Wait();
            return RedirectToAction("NewsFeedIndex", "Home");
#endif
            #endregion

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
                var userLogInAttempt = await SecurityDataProvider.UserLogIn(DbContext, model.Email, model.Password);

                //did we find a user?
                if (userLogInAttempt == null)
                {
                    //can't find the user, add an error
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                }
                else
                {
                    //go log the user in
                    await LogUserIn(userLogInAttempt);

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

        private static IList<BreadcrumbNavItem> BuildSignUpBreadcrumb()
        {
            var breadCrumb = new List<BreadcrumbNavItem>();

            breadCrumb.Add(new BreadcrumbNavItem("Home", "#"));
            breadCrumb.Add(new BreadcrumbNavItem("Sign Up", "#"));

            return breadCrumb;
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("SignUp", Name = "SignUp")]
        public IActionResult SignUp()
        {
            return View(new SignUpInViewModel(BuildSignUpBreadcrumb(),
                                              CacheFactory.GetCacheItem<IEnumerable<SelectListItem>>(CacheKeyNames.StateListing, Cache),
                                              new SignUpEnteredData { CurrentSeason = DateTime.Now.Year.ToString() }));
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("SignUp", Name = "SignUp")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SignUp(SignUpEnteredData model, IFormFile UserProfilePicture)
        {
            //do we have a valid model?
            if (ModelState.IsValid)
            {
                UserAccounts userRegisterAttempt = null;

                try
                {
                    //let's try to add this user to the system
                    userRegisterAttempt = await SecurityDataProvider.RegisterUser(DbContext, model);

                    //successful. go add the image if they have one
                    if (UserProfilePicture != null)
                    {
                        var tryToGetFileExtension = UserProfilePicture.ContentType.Split('/')[1];

                        await UserProfilePicture.SaveAsAsync(Path.Combine(Configuration.Value.UserImageSavePath, userRegisterAttempt.UserId.ToString() + "." + tryToGetFileExtension));

                        //go invalidate the user cache
                        CacheFactory.RemoveCacheItem(CacheKeyNames.UserImageFinder, Cache);
                    }
                }
                catch (Exception ex)
                {
                    var sqlException = ToracLibrary.Core.Exceptions.ExceptionUtilities.RetrieveExceptionType<SqlException>(ex);

                    //do we have a sql exception/* PK/UKC violation */
                    if (sqlException != null && sqlException.Errors.OfType<SqlError>().Any(x => x.Number == UniqueConstraintId))
                    {
                        // it's a dupe... do something about it
                        ModelState.AddModelError(string.Empty, "E-mail address is already registered.");
                    }
                    else
                    {
                        // it's something else...
                        throw;
                    }
                }

                //did we find a user?
                if (userRegisterAttempt != null)
                {
                    //go log the user in
                    await LogUserIn(userRegisterAttempt);

                    //go send them to the main page
                    return RedirectToAction("Index", "Home");
                }

                //if we don't have a duplicate email error, then just add a generic register error
                if (ModelState.ErrorCount == 0)
                {
                    //can't find the user, add an error
                    ModelState.AddModelError(string.Empty, "Invalid register attempt.");
                }
            }

            // If we got this far, something failed, redisplay form
            return View(new SignUpInViewModel(BuildSignUpBreadcrumb(),
                                             CacheFactory.GetCacheItem<IEnumerable<SelectListItem>>(CacheKeyNames.StateListing, Cache),
                                             model));
        }

        #endregion

    }
}
