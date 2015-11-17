﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Authorization;
using ToracGolf.ViewModels.Navigation;
using ToracGolf.Constants;
using ToracGolf.ViewModels.Settings;
using Microsoft.Framework.Caching.Memory;
using ToracLibrary.AspNet.Caching.FactoryStore;
using ToracGolf.MiddleLayer.EFModel;
using Microsoft.AspNet.Antiforgery;
using Microsoft.Framework.OptionsModel;
using ToracGolf.Settings;

namespace ToracGolf.Controllers
{

    [Authorize]
    public class SettingsController : BaseController
    {

        #region Constructor

        public SettingsController(IMemoryCache cache, ICacheFactoryStore cacheFactoryStore, Lazy<ToracGolfContext> dbContext, IAntiforgery antiforgery, IOptions<AppSettings> configuration)
        {
            DbContext = dbContext;
            Cache = cache;
            CacheFactory = cacheFactoryStore;
            Antiforgery = antiforgery;
            Configuration = configuration;
        }

        #endregion

        #region Properties

        private Lazy<ToracGolfContext> DbContext { get; }

        private IMemoryCache Cache { get; }

        private ICacheFactoryStore CacheFactory { get; }

        private IAntiforgery Antiforgery { get; }

        private IOptions<AppSettings> Configuration { get; }

        #endregion
        
        #region Methods

        [HttpGet]
        [Route("ChangeMySettings", Name = "ChangeMySettings")]      
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Route("ChangePassword", Name = "ChangePassword")]
        public async Task<IActionResult> ChangeMyPasswords()
        {
            //grab the user id
            var userId = GetUserId();

            var breadCrumb = new List<BreadcrumbNavItem>();

            breadCrumb.Add(new BreadcrumbNavItem("Home", ApplicationConstants.MainLandingPage));
            breadCrumb.Add(new BreadcrumbNavItem("Change My Password", "#"));

            return View("ChangePassword", new ChangePasswordViewModel(
                await HandicapStatusBuilder(DbContext.Value, userId, await UserCurrentSeason(DbContext.Value, userId)),
                breadCrumb,
                BuildTokenSet(Antiforgery)));
        }

        #endregion

    }

}
