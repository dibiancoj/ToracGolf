using Microsoft.AspNet.Antiforgery;
using Microsoft.AspNet.Authorization;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Mvc.Rendering;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.OptionsModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToracGolf.Constants;
using ToracGolf.Filters;
using ToracGolf.MiddleLayer.Courses;
using ToracGolf.MiddleLayer.EFModel;
using ToracGolf.MiddleLayer.GridCommon;
using ToracGolf.MiddleLayer.ListingFactories;
using ToracGolf.MiddleLayer.Rounds;
using ToracGolf.MiddleLayer.Rounds.Models;
using ToracGolf.MiddleLayer.Season;
using ToracGolf.Settings;
using ToracGolf.ViewModels.Navigation;
using ToracGolf.ViewModels.Rounds;
using ToracLibrary.AspNet.Caching.FactoryStore;
using ToracLibrary.AspNet.Paging;

namespace ToracGolf.Controllers
{

    [Authorize]
    public class RoundController : BaseController
    {

        #region Constructor

        public RoundController(IMemoryCache cache, 
                               ICacheFactoryStore cacheFactoryStore, 
                               ToracGolfContext dbContext, 
                               IAntiforgery antiforgery, 
                               IOptions<AppSettings> configuration,
                               IListingFactory<RoundListingData> roundListingFactory)
        {
            DbContext = dbContext;
            Cache = cache;
            CacheFactory = cacheFactoryStore;
            Antiforgery = antiforgery;
            Configuration = configuration;
            RoundListingFactory = roundListingFactory;
        }

        #endregion

        #region Properties

        private ToracGolfContext DbContext { get; }

        private IMemoryCache Cache { get; }

        private ICacheFactoryStore CacheFactory { get; }

        private IAntiforgery Antiforgery { get; }

        private IOptions<AppSettings> Configuration { get; }

        private IListingFactory<RoundListingData> RoundListingFactory { get; }

        #endregion

        #region Breadcrumbs

        private static IList<BreadcrumbNavItem> BaseBreadCrumb()
        {
            var breadCrumb = new List<BreadcrumbNavItem>();

            breadCrumb.Add(new BreadcrumbNavItem("Home", ApplicationConstants.MainLandingPage));
            breadCrumb.Add(new BreadcrumbNavItem("Rounds", ApplicationConstants.RoundListingRouteName));

            return breadCrumb;
        }

        #endregion

        #region Add A Round

        [HttpGet]
        [Route("AddARound", Name = "AddARound")]
        public async Task<IActionResult> RoundAdd()
        {
            //go build the breadcrumb
            var breadCrumb = BaseBreadCrumb();

            //add the current screen
            breadCrumb.Add(new BreadcrumbNavItem("Add A Round", "#"));

            //grab the user id and store it
            var userId = GetUserId();

            //go return the view
            return View(new RoundAddViewModel(
                await HandicapStatusBuilder(DbContext, userId, await UserCurrentSeason(DbContext, userId)),
                breadCrumb,
                CacheFactory.GetCacheItem<IEnumerable<SelectListItem>>(CacheKeyNames.StateListing, Cache),
                BuildTokenSet(Antiforgery),
                new RoundAddEnteredData { RoundDate = DateTime.Now, StateId = GetUserDefaultState() }));
        }

        [HttpPost]
        [Route("AddARound", Name = "AddARound")]
        [ValidateCustomAntiForgeryToken()]
        public async Task<IActionResult> RoundAdd([FromBody]RoundAddEnteredData model)
        {
            //do we have a valid model?
            if (ModelState.IsValid)
            {
                //make sure they don't have fairways hit more then the course allows
                if (model.FairwaysHit.HasValue)
                {
                    int maxFairwaysHitAllowed = await CourseDataProvider.TeeboxNumberOfFairways(DbContext, model.TeeLocationId);

                    if (model.FairwaysHit.Value > maxFairwaysHitAllowed)
                    {
                        ModelState.AddModelError(string.Empty, "Fairways Hit Must Be Less Than Or Equal To " + maxFairwaysHitAllowed);
                    }
                }

                //do we have any errors
                if (ModelState.ErrorCount == 0)
                {
                    //grab the user id
                    var userId = GetUserId();

                    //grab the users current season
                    var usersCurrentSeason = await UserCurrentSeason(DbContext, userId);

                    //let's try to add this user to the system
                    var roundAddAttempt = await RoundDataProvider.SaveRound(DbContext, userId, usersCurrentSeason, model);

                    //if we saved the round, we want to clear out the session so the next call which go calculate the handicap now
                    HttpContext.Session.Remove(HandicapStatusSessionName);

                    //we saved it successfully
                    return Json(new { result = true });
                }
            }

            //add a generic error if we don't have (this way we return something)
            if (ModelState.ErrorCount == 0)
            {
                ModelState.AddModelError(string.Empty, "Not Able To Save Round");
            }

            //return the error here
            return new BadRequestObjectResult(ModelState);
        }

        [HttpPost]
        [Route("CourseSelectByState", Name = "CourseSelectByState")]
        [ValidateCustomAntiForgeryToken()]
        public async Task<IActionResult> SelectCoursesForState([FromBody]int stateId)
        {
            //go grab the course listing
            return Json(new
            {
                CourseData = await RoundDataProvider.ActiveCoursesSelectForState(DbContext, stateId)
            });
        }

        [HttpPost]
        [Route("TeeLocationSelectForCourseId", Name = "TeeLocationSelectForCourseId")]
        [ValidateCustomAntiForgeryToken()]
        public async Task<IActionResult> SelectTeeBoxForCourseId([FromBody]int courseId)
        {
            //grab the user id
            var userId = GetUserId();

            //let's go grab the users handicap
            var usersHandicap = await HandicapStatusBuilder(DbContext, userId, await UserCurrentSeason(DbContext, userId));

            //go grab the course listing
            return Json(new
            {
                TeeBoxData = await RoundDataProvider.TeeBoxSelectForCourse(DbContext, courseId, usersHandicap.CareerHandicap)
            });
        }

        #endregion

        #region Round Listing

        [HttpGet]
        [Route("ViewRounds", Name = "ViewRounds")]
        public async Task<IActionResult> RoundListing()
        {
            //go build the breadcrumb
            var breadCrumb = BaseBreadCrumb();

            //add the current screen
            breadCrumb.Add(new BreadcrumbNavItem("Round Listing", "#"));

            //grab the user id and store it
            var userId = GetUserId();

            //let's grab the users season
            var userSeasons = BuildSelectList((await SeasonDataProvider.SeasonSelectForUser(DbContext, userId)),
                x => x.Key.ToString(),
                x => x.Value,
                () => new SelectListItem { Value = string.Empty, Text = "All Seasons" });

            //get the total number of rounds
            //var totalNumberOfRounds = await RoundDataProvider.TotalNumberOfRounds(DbContext, userId, null, null, null, null, false);

            //go return the view
            return View(new RoundListingViewModel(
                await HandicapStatusBuilder(DbContext, userId, await UserCurrentSeason(DbContext, userId)),
                breadCrumb,
                //CacheFactory.GetCacheItem<IEnumerable<SelectListItem>>(CacheKeyNames.StateListing, Cache),
                BuildTokenSet(Antiforgery),
                //GetUserDefaultState(),
                //DataSetPaging.CalculateTotalPages(totalNumberOfRounds, Configuration.Value.DefaultListingRecordsPerPage),
                //totalNumberOfRounds,
                CacheFactory.GetCacheItem<IList<SortOrderViewModel>>(CacheKeyNames.RoundListingSortOrder, Cache),
                Configuration.Value.DefaultListingRecordsPerPage,
                CacheFactory.GetCacheItem<IEnumerable<int>>(CacheKeyNames.NumberOfListingsPerPage, Cache),
                userSeasons));
        }

        [HttpPost]
        [Route("RoundListingSelectPage", Name = "RoundListingSelectPage")]
        [ValidateCustomAntiForgeryToken()]
        public async Task<IActionResult> RoundListingSelect([FromBody] RoundListPageNavigation listNav)
        {
            //grab the userid
            var userId = GetUserId();

            //state filter to use
            int? seasonFilter = string.IsNullOrEmpty(listNav.SeasonFilter) ? new int?() : Convert.ToInt32(listNav.SeasonFilter);

            int? totalNumberOfPages = null;
            int? totalNumberOfRecords = null;

            if (listNav.ResetPager)
            {
                totalNumberOfRecords = await RoundDataProvider.TotalNumberOfRounds(DbContext, userId, listNav.CourseNameFilter, seasonFilter, listNav.RoundDateStartFilter, listNav.RoundDateEndFilter, listNav.HandicappedRoundsOnly);

                totalNumberOfPages = DataSetPaging.CalculateTotalPages(totalNumberOfRecords.Value, listNav.RoundsPerPage);
            }

            return Json(new
            {
                PagedData = await RoundDataProvider.RoundSelect(RoundListingFactory, DbContext, userId, listNav.PageIndexId, listNav.SortBy, listNav.CourseNameFilter, seasonFilter, listNav.RoundsPerPage, listNav.RoundDateStartFilter, listNav.RoundDateEndFilter, CacheFactory.GetCacheItem<CourseImageFinder>(CacheKeyNames.CourseImageFinder, Cache), listNav.HandicappedRoundsOnly),
                TotalNumberOfPages = totalNumberOfPages,
                TotalNumberOfRecords = totalNumberOfRecords
            });
        }

        #endregion

        #region Delete A Round

        [HttpPost]
        [Route("RoundDelete", Name = "RoundDelete")]
        public async Task<IActionResult> DeleteARound([FromBody]int roundIdToDelete)
        {
            //go delete the round
            var result = await RoundDataProvider.DeleteARound(DbContext, roundIdToDelete);

            //we want to clear the handicap session data, so we calc it on the next call. We deleted a round so things could shift around
            HttpContext.Session.Remove(HandicapStatusSessionName);

            //grab the user id
            var userId = GetUserId();

            //go delete the record and return the result
            return Json(new
            {
                Result = result,
                NewHandicap = await HandicapStatusBuilder(DbContext, userId, await UserCurrentSeason(DbContext, userId))
            });
        }

        #endregion

    }
}