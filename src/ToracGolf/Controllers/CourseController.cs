﻿using Microsoft.AspNet.Antiforgery;
using Microsoft.AspNet.Authorization;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Mvc.Rendering;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.OptionsModel;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using ToracGolf.Constants;
using ToracGolf.Filters;
using ToracGolf.MiddleLayer;
using ToracGolf.MiddleLayer.Courses;
using ToracGolf.MiddleLayer.Courses.Models.CourseStats;
using ToracGolf.MiddleLayer.EFModel;
using ToracGolf.MiddleLayer.EFModel.Tables;
using ToracGolf.MiddleLayer.GridCommon;
using ToracGolf.MiddleLayer.ListingFactories;
using ToracGolf.MiddleLayer.Season;
using ToracGolf.Settings;
using ToracGolf.ViewModels.Courses;
using ToracGolf.ViewModels.Navigation;
using ToracLibrary.AspNet.Caching.FactoryStore;
using ToracLibrary.Core.Paging;

namespace ToracGolf.Controllers
{

    [Authorize]
    public class CourseController : BaseController
    {

        #region Constructor

        public CourseController(IMemoryCache cache,
                                ICacheFactoryStore cacheFactoryStore,
                                ToracGolfContext dbContext,
                                IAntiforgery antiforgery,
                                IOptions<AppSettings> configuration,
                                IListingFactory<CourseListingFactory.CourseListingSortEnum, Course, CourseListingData> courseListingFactory)
        {
            DbContext = dbContext;
            Cache = cache;
            CacheFactory = cacheFactoryStore;
            Antiforgery = antiforgery;
            Configuration = configuration;
            CourseListingFactory = courseListingFactory;
        }

        #endregion

        #region Properties

        private ToracGolfContext DbContext { get; }

        private IMemoryCache Cache { get; }

        private ICacheFactoryStore CacheFactory { get; }

        private IAntiforgery Antiforgery { get; }

        private IOptions<AppSettings> Configuration { get; }

        private IListingFactory<CourseListingFactory.CourseListingSortEnum, Course, CourseListingData> CourseListingFactory { get; }

        #endregion

        #region Breadcrumbs

        private static IList<BreadcrumbNavItem> BaseBreadCrumb()
        {
            var breadCrumb = new List<BreadcrumbNavItem>();

            breadCrumb.Add(new BreadcrumbNavItem("Home", ApplicationConstants.MainLandingPage));
            breadCrumb.Add(new BreadcrumbNavItem("Courses", ApplicationConstants.CourseListingRouteName));

            return breadCrumb;
        }

        #endregion

        #region Add A Course

        [HttpGet]
        [Route(ApplicationConstants.AddACourseRouteName, Name = ApplicationConstants.AddACourseRouteName)]
        public async Task<IActionResult> CourseAdd()
        {
            //build the breadcrumb
            var breadCrumb = BaseBreadCrumb();

            //add the current screen
            breadCrumb.Add(new BreadcrumbNavItem("Add A Course", "#"));

            //grab the user id
            var userId = GetUserId();

            //remove the course image finder cache
            CacheFactory.RemoveCacheItem(CacheKeyNames.CourseImageFinder, Cache);

            return View(new CourseAddViewModel(
                await HandicapStatusBuilder(DbContext, userId, await UserCurrentSeason(DbContext, userId)),
                breadCrumb,
                CacheFactory.GetCacheItem<IEnumerable<SelectListItem>>(CacheKeyNames.StateListing, Cache),
                new CourseAddEnteredData { StateListing = GetUserDefaultState(), TeeLocations = new List<CourseAddEnteredDataTeeLocations>() },
                BuildTokenSet(Antiforgery)));
        }

        [HttpPost]
        [Route(ApplicationConstants.AddACourseRouteName, Name = ApplicationConstants.AddACourseRouteName)]
        [ValidateCustomAntiForgeryToken()]
        public async Task<IActionResult> CourseAdd([FromBody]CourseAddEnteredData model)
        {
            //do we have a valid model?
            if (ModelState.IsValid)
            {
                try
                {
                    //let's try to add this user to the system
                    var courseAddAttempt = await CourseDataProvider.CourseAdd(DbContext, GetUserId(), model, Configuration.Value.CourseImageSavePath);

                    //we saved it successfully
                    return Json(new { result = true });
                }
                catch (Exception ex)
                {
                    var sqlException = ToracLibrary.Core.Exceptions.ExceptionUtilities.RetrieveExceptionType<SqlException>(ex);

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
        [Route("CourseSelect/{CourseId}", Name = "CourseSelect")]
        public IActionResult CourseListing(int courseId)
        {
            return RedirectToRoute(ApplicationConstants.CourseListingRouteName, new { CourseId = courseId });
        }

        [HttpGet]
        [Route(ApplicationConstants.CourseListingRouteName, Name = ApplicationConstants.CourseListingRouteName)]
        public async Task<IActionResult> CourseListing(int? CourseId)
        {
            //build the breadcrumb
            var breadCrumb = BaseBreadCrumb();

            breadCrumb.Add(new BreadcrumbNavItem("Course Listing", "#"));

            //grab the state listing
            var stateListing = CacheFactory.GetCacheItem<IEnumerable<SelectListItem>>(CacheKeyNames.StateListing, Cache).ToList();

            //add the "all"
            stateListing.Insert(0, new SelectListItem { Text = "All", Value = "" });

            //grab the user id
            var userId = GetUserId();

            //filters
            string courseNameFilter = string.Empty;
            string defaultState = GetUserDefaultState();

            //if we have a course id, go grab the name and state and set it
            if (CourseId.HasValue)
            {
                //go grab the course info
                var courseInfo = await CourseDataProvider.CourseNameAndState(DbContext, CourseId.Value);

                //set the course name and filter
                courseNameFilter = courseInfo.Item1;
                defaultState = courseInfo.Item2;
            }

            //get the total number of courses
            //var totalNumberOfCourses = await CourseDataProvider.TotalNumberOfCourses(DbContext, null, null);

            //return the view
            return View(new CourseListingViewModel(
              await HandicapStatusBuilder(DbContext, userId, await UserCurrentSeason(DbContext, userId)),
              breadCrumb,
              BuildTokenSet(Antiforgery),
              //DataSetPaging.CalculateTotalPages(totalNumberOfCourses, Configuration.Value.DefaultListingRecordsPerPage),
              //totalNumberOfCourses,
              CacheFactory.GetCacheItem<IList<SortOrderViewModel>>(CacheKeyNames.CourseListingSortOrder, Cache),
              stateListing,
              defaultState,
              Configuration.Value.DefaultListingRecordsPerPage,
              CacheFactory.GetCacheItem<IEnumerable<int>>(CacheKeyNames.NumberOfListingsPerPage, Cache),
              courseNameFilter));
        }

        [HttpPost]
        [Route("CourseListingSelectPage", Name = "CourseListingSelectPage")]
        [ValidateCustomAntiForgeryToken()]
        public async Task<IActionResult> CourseListingSelect([FromBody] CourseListPageNavigation listNav)
        {
            //state filter to use
            int? stateFilter = string.IsNullOrEmpty(listNav.StateFilter) ? new int?() : Convert.ToInt32(listNav.StateFilter);

            //total number of pages and records
            int? totalNumberOfPages = null;
            int? totalNumberOfRecords = null;

            if (listNav.ResetPager)
            {
                totalNumberOfRecords = await CourseDataProvider.TotalNumberOfCourses(DbContext, CourseListingFactory, listNav.CourseNameFilter, stateFilter);

                totalNumberOfPages = DataSetPaging.CalculateTotalPages(totalNumberOfRecords.Value, listNav.CoursesPerPage);
            }

            return Json(new
            {
                PagedData = await CourseDataProvider.CourseSelect(CourseListingFactory, DbContext, listNav.PageIndexId, listNav.SortBy, listNav.CourseNameFilter, stateFilter, listNav.CoursesPerPage, GetUserId(), CacheFactory.GetCacheItem<ImageFinder>(CacheKeyNames.CourseImageFinder, Cache)),
                TotalNumberOfPages = totalNumberOfPages,
                TotalNumberOfRecords = totalNumberOfRecords
            });
        }

        #endregion

        #region Course Delete

        [HttpPost]
        [Route("CourseDelete", Name = "CourseDelete")]
        [ValidateCustomAntiForgeryToken()]
        public async Task<IActionResult> DeleteACourse([FromBody] int courseId)
        {
            //go delete the course
            return Json(new
            {
                Result = await CourseDataProvider.DeleteACourse(DbContext, courseId)
            });
        }

        #endregion

        #region Course Stats

        [HttpGet]
        [Route("CourseStats/{CourseId}", Name = "CourseStats")]
        public async Task<IActionResult> CourseStatSelect(int CourseId)
        {
            //build the breadcrumb
            var breadCrumb = BaseBreadCrumb();

            //add the current screen
            breadCrumb.Add(new BreadcrumbNavItem("Course Stats", "#"));

            //grab the user id
            var userId = GetUserId();

            //let's grab the users season
            var userSeasons = (await SeasonDataProvider.SeasonSelectForUser(DbContext, userId));

            //grab the course
            var courseData = await CourseDataProvider.CourseStatsSelect(DbContext, CourseId, userId, CacheFactory.GetCacheItem<ImageFinder>(CacheKeyNames.CourseImageFinder, Cache));

            return View(new CourseStatsViewModel(
                await HandicapStatusBuilder(DbContext, userId, await UserCurrentSeason(DbContext, userId)),
                breadCrumb,
                BuildTokenSet(Antiforgery),
                courseData,
                BuildSelectList(userSeasons, x => x.Key.ToString(), x => x.Value, () => new SelectListItem { Value = string.Empty, Text = "All Seasons" }),
                BuildSelectList(courseData.TeeBoxLocations, x => x.TeeLocationId.ToString(), x => x.Name, () => new SelectListItem { Value = string.Empty, Text = "All Tee Boxes" }),
                await CourseDataProvider.CourseStatsQuery(DbContext, new CourseStatsQueryRequest { CourseId = CourseId }, userId)));
        }

        [HttpPost]
        [ValidateCustomAntiForgeryToken()]
        [Route("CourseStatQuery", Name = "CourseStatQuery")]
        public async Task<IActionResult> CourseStatQueryRecords(CourseStatsQueryRequest queryModel)
        {
            return Json(await CourseDataProvider.CourseStatsQuery(DbContext, queryModel, GetUserId()));
        }

        #endregion

    }
}
