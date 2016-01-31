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
using ToracGolf.MiddleLayer;
using ToracGolf.MiddleLayer.Courses;
using ToracGolf.MiddleLayer.Dashboard;
using ToracGolf.MiddleLayer.Dashboard.Models;
using ToracGolf.MiddleLayer.EFModel;
using ToracGolf.MiddleLayer.NewsFeed;
using ToracGolf.Settings;
using ToracGolf.ViewModels.Home;
using ToracGolf.ViewModels.Navigation;
using ToracGolf.ViewModels.NewsFeed;
using ToracLibrary.AspNet.Caching.FactoryStore;

namespace ToracGolf.Controllers
{

    [Authorize]
    public class HomeController : BaseController
    {

        #region Constructor

        public HomeController(IMemoryCache cache,
                              ICacheFactoryStore cacheFactoryStore,
                              ToracGolfContext dbContext,
                              IAntiforgery antiforgery,
                              IOptions<AppSettings> configuration,
                              Lazy<NewsFeedDataProvider> newsFeedDataProvider)
        {
            DbContext = dbContext;
            Cache = cache;
            CacheFactory = cacheFactoryStore;
            Antiforgery = antiforgery;
            Configuration = configuration;
            NewsFeedDataProvider = newsFeedDataProvider;
        }

        #endregion

        #region Properties

        private ToracGolfContext DbContext { get; }

        private IMemoryCache Cache { get; }

        private ICacheFactoryStore CacheFactory { get; }

        private IAntiforgery Antiforgery { get; }

        private IOptions<AppSettings> Configuration { get; }

        private Lazy<NewsFeedDataProvider> NewsFeedDataProvider { get; }

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

        #region Main Index For Default Route

        public ActionResult Index()
        {
            //*** leave this in here *** if you remove it the default route will be jacked up. so if you do http://toracgolfv2 it goes to a blank page
            return RedirectToRoute(ApplicationConstants.MainLandingPage);
        }

        #endregion

        #region Dashboard

        [Route(ApplicationConstants.MainLandingPage, Name = ApplicationConstants.MainLandingPage)]
        [HttpGet]
        public async Task<IActionResult> DashboardIndex()
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

        [Route("NewsFeedClientView", Name = "NewsFeedClientView")]
        [HttpGet]
        public IActionResult NewsFeedPostClientView()
        {
            return View();
        }

        [Route("NewsFeedItemClientView", Name = "NewsFeedItemClientView")]
        [HttpGet]
        public IActionResult NewsFeedPostClientViewItem()
        {
            return View();
        }

        [Route("NewsFeedCommentItemClientView", Name = "NewsFeedCommentItemClientView")]
        [HttpGet]
        public IActionResult NewsFeedPostCommentClientView()
        {
            return View();
        }

        [ValidateCustomAntiForgeryToken]
        [Route("NewsFeedsGetPost", Name = "NewsFeedsGetPost")]
        [HttpPost]
        public async Task<IActionResult> NewsFeedGet([FromBody]NewsFeedGetRequest filterParams)
        {
            return Json(await NewsFeedDataProvider.Value.NewsFeedPostSelect(
                GetUserId(),
                filterParams.NewsFeedTypeIdFilter,
                filterParams.SearchFilterText,
                CacheFactory.GetCacheItem<ImageFinder>(CacheKeyNames.CourseImageFinder, Cache),
                CacheFactory.GetCacheItem<ImageFinder>(CacheKeyNames.UserImageFinder, Cache)));
        }

        [ValidateCustomAntiForgeryToken]
        [Route("NewsFeedsLike", Name = "NewsFeedsLike")]
        [HttpPost]
        public async Task<IActionResult> NewsFeedLike([FromBody]NewsFeedAddLike likeModel)
        {
            return Json(await NewsFeedDataProvider.Value.NewsFeedLikeAdd(DbContext, GetUserId(), likeModel.Id, likeModel.NewsFeedTypeId));
        }

        [ValidateCustomAntiForgeryToken]
        [Route("NewsFeedCommentSave", Name = "NewsFeedCommentSave")]
        [HttpPost]
        public async Task<IActionResult> NewsFeedCommentSaveRecord([FromBody]NewsFeedAddComment commentModel)
        {
            //go save the comment
            return Json(await NewsFeedDataProvider.Value.CommentAdd(DbContext, GetUserId(), commentModel.Id, commentModel.NewsFeedTypeId, commentModel.CommentToAdd, CacheFactory.GetCacheItem<ImageFinder>(CacheKeyNames.UserImageFinder, Cache)));
        }

        [ValidateCustomAntiForgeryToken]
        [Route("NewsFeedsCommentSelect", Name = "NewsFeedsCommentSelect")]
        [HttpPost]
        public async Task<IActionResult> NewsFeedCommentGet([FromBody]NewsFeedCommentSelect commentModel)
        {
            //go save the comment
            return Json(await NewsFeedDataProvider.Value.CommentSelect(DbContext, GetUserId(), commentModel.Id, commentModel.NewsFeedTypeId, CacheFactory.GetCacheItem<ImageFinder>(CacheKeyNames.UserImageFinder, Cache)));
        }

        [ValidateCustomAntiForgeryToken]
        [Route("NewsFeedsCommentLike", Name = "NewsFeedsCommentLike")]
        [HttpPost]
        public async Task<IActionResult> NewsFeedCommentLikeAddOrRemove([FromBody]NewsFeedCommentLike commentModel)
        {
            //go save the comment
            return Json(await NewsFeedDataProvider.Value.CommentLikeAddOrRemove(DbContext, GetUserId(), commentModel.CommentId));
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
