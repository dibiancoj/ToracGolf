﻿using Microsoft.AspNet.Antiforgery;
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

            breadCrumb.Add(new BreadcrumbNavItem("Home", "/"));
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

            

                //let's try to add this user to the system
                //var roundAddAttempt = await SeasonDataProvider..SaveRound(DbContext, userId, usersCurrentSeason, model);

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

    }

}