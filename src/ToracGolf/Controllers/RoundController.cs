﻿using Microsoft.AspNet.Antiforgery;
using Microsoft.AspNet.Authorization;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Mvc.Rendering;
using Microsoft.Framework.Caching.Memory;
using Microsoft.Framework.OptionsModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ToracGolf.Constants;
using ToracGolf.Filters;
using ToracGolf.MiddleLayer.EFModel;
using ToracGolf.MiddleLayer.Rounds;
using ToracGolf.MiddleLayer.Season;
using ToracGolf.Settings;
using ToracGolf.ViewModels.Navigation;
using ToracGolf.ViewModels.Rounds;
using ToracLibrary.AspNet.Caching.FactoryStore;

namespace ToracGolf.Controllers
{

    [Authorize]
    public class RoundController : BaseController
    {

        #region Constructor

        public RoundController(IMemoryCache cache, ICacheFactoryStore cacheFactoryStore, ToracGolfContext dbContext, IAntiforgery antiforgery, IOptions<AppSettings> configuration)
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
            breadCrumb.Add(new BreadcrumbNavItem("Rounds", ApplicationConstants.RoundListingRouteName));

            return breadCrumb;
        }

        #endregion

        #region Add A Round

        [HttpGet]
        [Route("AddARound", Name = "AddARound")]
        public IActionResult RoundAdd()
        {
            //go build the breadcrumb
            var breadCrumb = BaseBreadCrumb();

            //add the current screen
            breadCrumb.Add(new BreadcrumbNavItem("Add A Round", "#"));

            //get the user's preference, so we can the state he will most likely add
            var usersDefaultState = Context.User.Claims.First(x => x.Type == ClaimTypes.StateOrProvince).Value;

            //go return the view
            return View(new RoundAddViewModel(
                breadCrumb,
                CacheFactory.GetCacheItem<IEnumerable<SelectListItem>>(CacheKeyNames.StateListing, Cache),
                BuildTokenSet(Antiforgery),
                new RoundAddEnteredData() { RoundDate = DateTime.Now, StateId = Context.User.Claims.First(x => x.Type == ClaimTypes.StateOrProvince).Value }));
        }

        [HttpPost]
        [Route("AddARound", Name = "AddARound")]
        [ValidateCustomAntiForgeryToken()]
        public async Task<IActionResult> RoundAdd([FromBody]RoundAddEnteredData model)
        {
            //do we have a valid model?
            if (ModelState.IsValid)
            {
                //grab the user id
                var userId = GetUserId();

                //let's try to add this user to the system
                var roundAddAttempt = await RoundDataProvider.SaveRound(DbContext, userId, await SeasonDataProvider.CurrentSeasonForUser(DbContext, userId), model);

                //we saved it successfully
                return Json(new { result = true });
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
        public async Task<IActionResult> SelectCoursesForState([FromBody]CourseSelectByStateId model)
        {
            //go grab the course listing
            return Json(new
            {
                CourseData = await RoundDataProvider.CoursesSelectForState(DbContext, model.StateId)
            });
        }

        [HttpPost]
        [Route("TeeLocationSelectForCourseId", Name = "TeeLocationSelectForCourseId")]
        [ValidateCustomAntiForgeryToken()]
        public async Task<IActionResult> SelectTeeBoxForCourseId([FromBody]TeeBoxSelectByCourseId model)
        {
            //go grab the course listing
            return Json(new
            {
                TeeBoxData = await RoundDataProvider.TeeBoxSelectForCourse(DbContext, model.CourseId)
            });
        }

        #endregion

    }
}