using Microsoft.AspNet.Antiforgery;
using Microsoft.AspNet.Authorization;
using Microsoft.AspNet.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.OptionsModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToracGolf.Constants;
using ToracGolf.Filters;
using ToracGolf.MiddleLayer.Dashboard;
using ToracGolf.MiddleLayer.Dashboard.Models;
using ToracGolf.MiddleLayer.EFModel;
using ToracGolf.Settings;
using ToracGolf.ViewModels.Home;
using ToracGolf.ViewModels.Navigation;
using ToracLibrary.AspNet.Caching.FactoryStore;

namespace ToracGolf.Controllers
{

    [Authorize]
    public class HomeController : BaseController
    {

        #region Constructor

        public HomeController(IMemoryCache cache, ICacheFactoryStore cacheFactoryStore, ToracGolfContext dbContext, IAntiforgery antiforgery, IOptions<AppSettings> configuration)
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

        #region Methods

        #region Breadcrumbs

        private static IList<BreadcrumbNavItem> BaseBreadCrumb()
        {
            var breadCrumb = new List<BreadcrumbNavItem>();

            breadCrumb.Add(new BreadcrumbNavItem("Home", ApplicationConstants.MainLandingPage));

            return breadCrumb;
        }

        #endregion

        #region Dashboard

        [Route(ApplicationConstants.MainLandingPage, Name = ApplicationConstants.MainLandingPage)]
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            //grab the user id
            var userId = GetUserId();

            var breadCrumb = BaseBreadCrumb();

            //add the current screen
            breadCrumb.Add(new BreadcrumbNavItem("Dashboard", "#"));

            return View("Index", new IndexViewModel(
                await HandicapStatusBuilder(DbContext, userId, await UserCurrentSeason(DbContext, userId)),
                breadCrumb,
                BuildTokenSet(Antiforgery),
                DashboardViewType.DashboardViewTypeEnum.Career));
        }

        [HttpPost]
        [Route("DashboardViewTypeChange", Name = "DashboardViewTypeChange")]
        [ValidateCustomAntiForgeryToken()]
        public async Task<IActionResult> DashboardChangeViewType([FromBody]ViewTypeChangeViewModel Model)
        {
            //grab the user id
            var userId = GetUserId();

            //get the users default season
            int? seasonId = Model.ViewType == DashboardViewType.DashboardViewTypeEnum.Career ? await Task.FromResult<Int32?>(null) : await UserCurrentSeason(DbContext, userId);

            //go grab the data
            return Json(new
            {
                Last5Rounds = await DashboardDataProvider.Last5RoundsSelect(DbContext, userId, seasonId),
                Top5Rounds = await DashboardDataProvider.Top5RoundsSelect(DbContext, userId, seasonId),
                HandicapScoreSplitGrid = await DashboardDataProvider.ScoreHandicapGraph(DbContext, userId, seasonId),
                RoundPieChart = (await DashboardDataProvider.RoundPieChart(DbContext, userId, seasonId)).Select(x => new object[] { x.Key, x.Value })
            });
        }

        #endregion

        #region News Feed

        [Route(ApplicationConstants.NewsFeed, Name = ApplicationConstants.NewsFeed)]
        [HttpGet]
        public async Task<IActionResult> NewsFeedIndex()
        {
            //grab the user id
            var userId = GetUserId();

            var breadCrumb = new List<BreadcrumbNavItem>();

            breadCrumb.Add(new BreadcrumbNavItem("Home", ApplicationConstants.MainLandingPage));
            breadCrumb.Add(new BreadcrumbNavItem("News Feed", "#"));

            return View(new NewsFeedViewModel(
                await HandicapStatusBuilder(DbContext, userId, await UserCurrentSeason(DbContext, userId)),
                breadCrumb,
                BuildTokenSet(Antiforgery)));
        }

        #endregion

        #region Error Handling

        public IActionResult Error()
        {
            return View("~/Views/Shared/Error.cshtml");
        }

        #endregion

        #endregion

    }

}
