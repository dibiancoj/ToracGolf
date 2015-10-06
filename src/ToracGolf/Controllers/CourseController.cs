using Microsoft.AspNet.Antiforgery;
using Microsoft.AspNet.Authorization;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Mvc.Rendering;
using Microsoft.Framework.Caching.Memory;
using Microsoft.Framework.OptionsModel;
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
using ToracGolf.Settings;
using ToracGolf.ViewModels.Courses;
using ToracGolf.ViewModels.Navigation;
using ToracLibrary.AspNet.Caching.FactoryStore;

namespace ToracGolf.Controllers
{

    [Authorize]
    public class CourseController : BaseController
    {

        #region Constructor

        public CourseController(IMemoryCache cache, ICacheFactoryStore cacheFactoryStore, ToracGolfContext dbContext, IAntiforgery antiforgery, IOptions<AppSettings> configuration)
        {
            DbContext = dbContext;
            Cache = cache;
            CacheFactory = cacheFactoryStore;
            Antiforgery = antiforgery;
            Configuration = configuration;
        }

        #endregion

        #region Properties

        private ToracGolfContext DbContext { get; }

        private IMemoryCache Cache { get; }

        private ICacheFactoryStore CacheFactory { get; }

        private IAntiforgery Antiforgery { get; }

        private IOptions<AppSettings> Configuration { get; }

        #endregion

        #region Breadcrumbs

        private static IList<BreadcrumbNavItem> BaseBreadCrumb()
        {
            var breadCrumb = new List<BreadcrumbNavItem>();

            breadCrumb.Add(new BreadcrumbNavItem("Home", "/"));
            breadCrumb.Add(new BreadcrumbNavItem("Courses", ApplicationConstants.CourseListingRouteName));

            return breadCrumb;
        }

        #endregion

        #region Add A Course

        [HttpGet]
        [Route("AddACourse", Name = "AddACourse")]
        public IActionResult CourseAdd()
        {
            //build the breadcrumb
            var breadCrumb = BaseBreadCrumb();

            //add the current screen
            breadCrumb.Add(new BreadcrumbNavItem("Add A Course", "#"));

            //get the user's preference, so we can the state he will most likely add
            var usersDefaultState = Context.User.Claims.First(x => x.Type == ClaimTypes.StateOrProvince).Value;

            return View(new CourseAddViewModel(
                breadCrumb,
                CacheFactory.GetCacheItem<IEnumerable<SelectListItem>>(CacheKeyNames.StateListing, Cache),
                new CourseAddEnteredData { StateListing = usersDefaultState, TeeLocations = new List<CourseAddEnteredDataTeeLocations>() },
                BuildTokenSet(Antiforgery)));
        }

        [HttpPost]
        [Route("AddACourse", Name = "AddACourse")]
        [ValidateCustomAntiForgeryToken()]
        public async Task<IActionResult> CourseAdd([FromBody]CourseAddEnteredData model)
        {
            //do we have a valid model?
            if (ModelState.IsValid)
            {
                try
                {
                    //let's try to add this user to the system
                    var courseAddAttempt = await Courses.CourseAdd(DbContext, GetUserId(), model);

                    //we saved it successfully
                    return Json(new { result = true });
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

        #region Tee Locations

        [HttpPost]
        [Route("ValidateTeeLocation", Name = "ValidateTeeLocation")]
        [ValidateCustomAntiForgeryToken()]
        public IActionResult ValidateTeeLocation([FromBody]CourseAddEnteredDataTeeLocations teeLocationToValidate)
        {
            //specifically calling this so we can validate the tee location and get the model state
            if (ModelState.ErrorCount == 0)
            {
                //we are ok, just return true
                return Json(true);
            }

            //we have errors, return the model state
            return new BadRequestObjectResult(ModelState);
        }

        #endregion

        #region Course Listing

        [HttpGet]
        [Route(ApplicationConstants.CourseListingRouteName, Name = ApplicationConstants.CourseListingRouteName)]
        public async Task<IActionResult> CourseListing()
        {
            //build the breadcrumb
            var breadCrumb = BaseBreadCrumb();

            breadCrumb.Add(new BreadcrumbNavItem("Course Listing", "#"));

            //grab the state listing
            var stateListing = CacheFactory.GetCacheItem<IEnumerable<SelectListItem>>(CacheKeyNames.StateListing, Cache).ToList();

            //add the "all"
            stateListing.Insert(0, new SelectListItem { Text = "All", Value = "" });

            //return the view
            return View(new CourseListingViewModel(
              breadCrumb,
              BuildTokenSet(Antiforgery),
              await Courses.TotalNumberOfCourses(DbContext, null, null, Configuration.Options.CourseListingRecordsPerPage),
              CacheFactory.GetCacheItem<IList<CourseListingSortOrderModel>>(CacheKeyNames.CourseListingSortOrder, Cache).ToArray(),
              stateListing,
              Context.User.Claims.First(x => x.Type == ClaimTypes.StateOrProvince).Value));
        }

        [HttpPost]
        [Route("CourseListingSelectPage", Name = "CourseListingSelectPage")]
        [ValidateCustomAntiForgeryToken()]
        public async Task<IActionResult> CourseListingSelect([FromBody] CourseListPageNavigation listNav)
        {
            //state filter to use
            int? stateFilter = string.IsNullOrEmpty(listNav.StateFilter) ? new int?() : Convert.ToInt32(listNav.StateFilter);

            return Json(new
            {
                PagedData = await Courses.CourseSelect(DbContext, listNav.PageIndexId, listNav.SortBy, listNav.CourseNameFilter, stateFilter, Configuration.Options.CourseListingRecordsPerPage),
                TotalNumberOfPages = listNav.ResetPager ? new int?(await Courses.TotalNumberOfCourses(DbContext, listNav.CourseNameFilter, stateFilter, Configuration.Options.CourseListingRecordsPerPage)) : null
            });
        }

        #endregion

    }
}
