using Microsoft.AspNet.Antiforgery;
using Microsoft.AspNet.Authorization;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Mvc.Rendering;
using Microsoft.Framework.Caching.Memory;
using Microsoft.Framework.OptionsModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToracGolf.Constants;
using ToracGolf.Filters;
using ToracGolf.MiddleLayer.EFModel;
using ToracGolf.MiddleLayer.Season;
using ToracGolf.Settings;
using ToracGolf.ViewModels.Navigation;
using ToracGolf.ViewModels.Season;
using ToracLibrary.AspNet.Caching.FactoryStore;

namespace ToracGolf.Controllers
{

    [Authorize]
    public class SeasonController : BaseController
    {

        #region Constructor

        public SeasonController(IMemoryCache cache, ICacheFactoryStore cacheFactoryStore, ToracGolfContext dbContext, IAntiforgery antiforgery, IOptions<AppSettings> configuration)
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

            breadCrumb.Add(new BreadcrumbNavItem("Home", ApplicationConstants.MainLandingPage));
            breadCrumb.Add(new BreadcrumbNavItem("Seasons", ApplicationConstants.SeasonListingRouteName));

            return breadCrumb;
        }

        #endregion

        #region Add A Round

        [HttpGet]
        [Route("AddASeason", Name = "AddASeason")]
        public async Task<IActionResult> SeasonAdd()
        {
            //go build the breadcrumb
            var breadCrumb = BaseBreadCrumb();

            //add the current screen
            breadCrumb.Add(new BreadcrumbNavItem("Add A Season", "#"));

            //grab the user id and store it
            var userId = GetUserId();

            //go return the view
            return View(new SeasonAddViewModel(
                await HandicapStatusBuilder(DbContext, userId, await UserCurrentSeason(DbContext, userId)),
                breadCrumb,
                BuildTokenSet(Antiforgery),
                new SeasonAddEnteredData()));
        }

        [HttpPost]
        [Route("AddASeason", Name = "AddASeason")]
        [ValidateCustomAntiForgeryToken()]
        public async Task<IActionResult> SeasonAdd([FromBody]SeasonAddEnteredData model)
        {
            //do we have a valid model?
            if (ModelState.IsValid)
            {
                //grab the user id
                var userId = GetUserId();

                //let's try to add this season to the system
                var refSeasonRecord = await SeasonDataProvider.RefSeasonAddOrGet(DbContext, model.SeasonDescription);

                //add the user season now
                var userSeason = await SeasonDataProvider.UserSeasonAdd(DbContext, userId, refSeasonRecord);

                //do we want to make this our current season
                if (model.MakeCurrentSeason)
                {
                    //go save the current season
                    await SeasonDataProvider.MakeSeasonAsCurrent(DbContext, userId, userSeason.SeasonId);
                }

                //we have a new current season, so we need to clear out the session
                Context.Session.Remove(HandicapStatusSessionName);

                //we saved it successfully
                return Json(new { result = true });
            }

            //add a generic error if we don't have (this way we return something)
            if (ModelState.ErrorCount == 0)
            {
                ModelState.AddModelError(string.Empty, "Not Able To Save New Season");
            }

            //return the error here
            return new BadRequestObjectResult(ModelState);
        }

        #endregion

        #region Season Listing

        [HttpGet]
        [Route("SeasonListing", Name = "SeasonListing")]
        public async Task<IActionResult> SeasonListing()
        {
            //go build the breadcrumb
            var breadCrumb = BaseBreadCrumb();

            //add the current screen
            breadCrumb.Add(new BreadcrumbNavItem("Season Listing", "#"));

            //grab the user id and store it
            var userId = GetUserId();

            //go return the view
            return View(new SeasonListingViewModel(
                await HandicapStatusBuilder(DbContext, userId, await UserCurrentSeason(DbContext, userId)),
                breadCrumb,
                BuildTokenSet(Antiforgery)));
        }

        [HttpPost]
        [Route("SeasonListing", Name = "SeasonListing")]
        [ValidateCustomAntiForgeryToken()]
        public async Task<IActionResult> SeasonListingGrid()
        {
            //go grab the data
            return Json(new
            {
                PagedData = (await SeasonDataProvider.SeasonListingForGrid(DbContext, GetUserId()))
            });
        }

        #endregion

        #region Delete A Season

        [HttpPost]
        [Route("SeasonDelete", Name = "SeasonDelete")]
        [ValidateCustomAntiForgeryToken()]
        public async Task<IActionResult> SeasonDelete([FromBody]int seasonId)
        {
            //grab the user id
            var userId = GetUserId();

            //go delete the season
            await SeasonDataProvider.DeleteSeason(DbContext, userId, seasonId);

            //we will return the paged data so we don't have to come back to the controller
            return Json(new
            {
                PagedData = await SeasonDataProvider.SeasonListingForGrid(DbContext, userId)
            });
        }

        #endregion

    }

}