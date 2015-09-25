using Microsoft.AspNet.Antiforgery;
using Microsoft.AspNet.Authorization;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Mvc.Rendering;
using Microsoft.Framework.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ToracGolf.Constants;
using ToracGolf.Filters;
using ToracGolf.MiddleLayer.Courses;
using ToracGolf.MiddleLayer.EFModel;
using ToracGolf.ViewModels.Courses;
using ToracGolf.ViewModels.Navigation;
using ToracLibrary.AspNet.Caching.FactoryStore;

namespace ToracGolf.Controllers
{

    [Authorize]
    public class CourseController : BaseController
    {

        #region Constructor

        public CourseController(IMemoryCache cache, ICacheFactoryStore cacheFactoryStore, ToracGolfContext dbContext, IAntiforgery antiforgery)
        {
            DbContext = dbContext;
            Cache = cache;
            CacheFactory = cacheFactoryStore;
            Antiforgery = antiforgery;
        }

        #endregion

        #region Properties

        private ToracGolfContext DbContext { get; }

        private IMemoryCache Cache { get; }

        private ICacheFactoryStore CacheFactory { get; }

        private IAntiforgery Antiforgery { get; }

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

            var token = Antiforgery.GetAndStoreTokens(Context);

            return View(new CourseAddViewModel(
                BuildAddACourseBreadcrumb(),
                CacheFactory.GetCacheItem<IEnumerable<SelectListItem>>(CacheKeyNames.StateListing, Cache),
                new CourseAddEnteredData { StateListing = usersDefaultState, TeeLocations = new List<CourseAddEnteredDataTeeLocations>() },
                token));
        }

        [HttpPost]
        [Route("AddACourse", Name = "AddACourse")]
        [ValidateCustomAntiForgeryToken()]
        public async Task<IActionResult> CourseAdd([FromBody]CourseAddEnteredData model)
        {
           //when published, the token in chrome and IE is not working. Doesn't look like its getting the cookie token. Need to research why!


            //do we have a valid model?
            if (ModelState.IsValid)
            {
                try
                {
                    //let's try to add this user to the system
                    var courseAddAttempt = await Courses.CourseAdd(DbContext, GetUserId(), model);

                    //we saved it successfully
                    return Json(new { id = 5 });
                }
                catch (Exception ex)
                {
                    var sqlException = ToracLibrary.AspNet.ExceptionHelpers.ExceptionUtilities.RetrieveExceptionType<SqlException>(ex);

                    //do we have a sql exception/* PK/UKC violation */
                    if (sqlException != null && sqlException.Errors.OfType<SqlError>().Any(x => x.Number == UniqueConstraintId))
                    {
                        // it's a dupe... do something about it
                        ModelState.AddModelError(string.Empty, "Course Name Is Already Registered.");
                    }
                    else
                    {
                        // it's something else...
                        throw;
                    }
                }
            }

            //add a generic error if we don't have (this way we return something)
            if (ModelState.ErrorCount == 0)
            {
                ModelState.AddModelError(string.Empty, "Not Able To Save Course");
            }

            //return the error here
            return new BadRequestObjectResult(ModelState);
        }

        #endregion

    }
}
