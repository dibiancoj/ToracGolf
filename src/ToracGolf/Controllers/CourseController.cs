using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Authorization;
using ToracGolf.ViewModels.Navigation;
using ToracGolf.ViewModels.Courses;
using ToracGolf.MiddleLayer.EFModel;
using Microsoft.Framework.Caching.Memory;
using ToracLibrary.AspNet.Caching.FactoryStore;
using Microsoft.AspNet.Mvc.Rendering;
using ToracGolf.Constants;
using System.Security.Claims;

namespace ToracGolf.Controllers
{

    [Authorize]
    public class CourseController : BaseController
    {

        #region Constructor

        public CourseController(IMemoryCache cache, ICacheFactoryStore cacheFactoryStore, ToracGolfContext dbContext)
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

        #region Add A Course

        private static IList<BreadcrumbNavItem> BuildAddACourseBreadcrumb()
        {
            var breadCrumb = new List<BreadcrumbNavItem>();

            breadCrumb.Add(new BreadcrumbNavItem("Home", "/"));
            breadCrumb.Add(new BreadcrumbNavItem("Courses", "#"));
            breadCrumb.Add(new BreadcrumbNavItem("Add A Course", "#"));

            return breadCrumb;
        }

        [HttpGet]
        [Route("AddACourse", Name = "AddACourse")]
        public IActionResult CourseAdd()
        {
            //get the user's preference, so we can the state he will most likely add
            var usersDefaultState = Context.User.Claims.First(x => x.Type == ClaimTypes.StateOrProvince).Value;

            return View(new CourseAddViewModel(
                BuildAddACourseBreadcrumb(),
                CacheFactory.GetCacheItem<IEnumerable<SelectListItem>>(CacheKeyNames.StateListing, Cache),
                new CourseAddEnteredData() { StateListing = usersDefaultState }));
        }

        [HttpPost]
        [Route("AddACourse", Name = "AddACourse")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SignUp(CourseAddEnteredData model)
        {
            //do we have a valid model?
            //if (ModelState.IsValid)
            //{
            //    UserAccounts userRegisterAttempt = null;

            //    try
            //    {
            //        //let's try to add this user to the system
            //        userRegisterAttempt = await Security.RegisterUser(DbContext, model);
            //    }
            //    catch (Exception ex)
            //    {
            //        var sqlException = ToracLibrary.Core.Exceptions.ExceptionUtilities.RetrieveExceptionType<SqlException>(ex);

            //        //do we have a sql exception/* PK/UKC violation */
            //        if (sqlException != null && sqlException.Errors.OfType<SqlError>().Any(x => x.Number == 2627))
            //        {
            //            // it's a dupe... do something about it
            //            ModelState.AddModelError(string.Empty, "E-mail address is already registered.");
            //        }
            //        else
            //        {
            //            // it's something else...
            //            throw;
            //        }
            //    }

            //    //did we find a user?
            //    if (userRegisterAttempt != null)
            //    {
            //        //go log the user in
            //        await LogUserIn(userRegisterAttempt);

            //        //go send them to the main page
            //        return RedirectToAction("Index", "Home");
            //    }

            //    //if we don't have a duplicate email error, then just add a generic register error
            //    if (ModelState.ErrorCount == 0)
            //    {
            //        //can't find the user, add an error
            //        ModelState.AddModelError(string.Empty, "Invalid register attempt.");
            //    }
            //}

            // If we got this far, something failed, redisplay form
            return View(new CourseAddViewModel(BuildAddACourseBreadcrumb(),
                                             CacheFactory.GetCacheItem<IEnumerable<SelectListItem>>(CacheKeyNames.StateListing, Cache),
                                             model));
        }

        #endregion

    }
}
