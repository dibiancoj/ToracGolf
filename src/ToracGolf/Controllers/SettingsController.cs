﻿using Microsoft.AspNet.Antiforgery;
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
using ToracGolf.MiddleLayer.EFModel;
using ToracGolf.MiddleLayer.SecurityManager;
using ToracGolf.Settings;
using ToracGolf.ViewModels.Navigation;
using ToracGolf.ViewModels.Settings;
using ToracLibrary.AspNet.Caching.FactoryStore;

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

        [HttpPost]
        [Route("ChangePassword", Name = "ChangePassword")]
        [ValidateCustomAntiForgeryToken()]
        public async Task<IActionResult> ChangePassword(ChangePasswordAttemptViewModel changePasswordViewModel)
        {
            if (ModelState.IsValid)
            {
                var userId = GetUserId();

                //need to add validation to check the old password is valid.
                var oldPassword = await SecurityDataProvider.Password(DbContext.Value, userId);

                //check the old vs current password
                if (!string.Equals(oldPassword, changePasswordViewModel.CurrentPW, StringComparison.OrdinalIgnoreCase))
                {
                    ModelState.AddModelError(string.Empty, "Current Password Does Not Match");

                    return new BadRequestObjectResult(ModelState);
                }

                //go change the password now
                await SecurityDataProvider.ChangePassword(DbContext.Value, userId, changePasswordViewModel.NewPw1);

                //return the result
                return Json(new { result = true });
            }

            //return the error here
            return new BadRequestObjectResult(ModelState);
        }

        #endregion

    }

}
