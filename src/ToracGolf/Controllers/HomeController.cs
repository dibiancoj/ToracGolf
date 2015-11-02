using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using ToracGolf.ViewModels.Home;
using ToracGolf.ViewModels.Navigation;
using ToracGolf.MiddleLayer.EFModel.Tables;
using System.Security.Claims;
using ToracGolf.Constants;
using Microsoft.Framework.Caching.Memory;
using ToracLibrary.AspNet.Caching.FactoryStore;
using ToracGolf.MiddleLayer.EFModel;
using Microsoft.AspNet.Antiforgery;
using Microsoft.Framework.OptionsModel;
using ToracGolf.Settings;
using Microsoft.AspNet.Authorization;
using ToracGolf.MiddleLayer.Dashboard;
using ToracGolf.MiddleLayer.Dashboard.Models;
using ToracGolf.Filters;

namespace ToracGolf.Controllers
{

    //[Authorize]
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

        //[Route("Home", Name = "Home")]
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            //let's try to log this user in
            var userLogInAttempt = (new UserAccounts { UserId = 21, EmailAddress = "dibiancoj@gmail.com", Password = "0", StateId = 35 });// await Security.UserLogIn(DbContext, model.Email, model.Password);

            var claims = new List<Claim>();

            claims.Add(new Claim(ClaimTypes.Hash, userLogInAttempt.UserId.ToString()));
            claims.Add(new Claim(ClaimTypes.Name, userLogInAttempt.FirstName + " " + userLogInAttempt.LastName));
            claims.Add(new Claim(ClaimTypes.Email, userLogInAttempt.EmailAddress));
            claims.Add(new Claim(ClaimTypes.StateOrProvince, userLogInAttempt.StateId.ToString()));

            //sign the user in
            await Context.Authentication.SignInAsync(SecuritySettings.SecurityType, new ClaimsPrincipal(new ClaimsIdentity(claims, SecuritySettings.SecurityType)));

            //return RedirectToRoute("SeasonListing");


            const int userId = 21;





            //grab the user id
            //var userId = GetUserId();

            var breadCrumb = new List<BreadcrumbNavItem>();

            breadCrumb.Add(new BreadcrumbNavItem("Home", "#"));

            return View("Index", new IndexViewModel(
                await HandicapStatusBuilder(DbContext, userId, await UserCurrentSeason(DbContext, userId)),
                breadCrumb,
                BuildTokenSet(Antiforgery),
                DashboardViewType.DashboardViewTypeEnum.Career));
        }

        [HttpPost]
        [Route("DashboardViewTypeChange", Name = "DashboardViewTypeChange")]
       // [ValidateCustomAntiForgeryToken()]
        public async Task<IActionResult> DashboardViewTypeChange([FromBody]ViewTypeChangeViewModel Model)
        {
            //grab the user id
            var userId = GetUserId();

            //get the users default season
            int? seasonId = Model.ViewType == DashboardViewType.DashboardViewTypeEnum.Career ? await Task.FromResult<Int32?>(null) : await UserCurrentSeason(DbContext, userId);

            //go grab the data
            return Json(new
            {
                Last5Rounds = await DashboardDataProvider.Last5RoundsSelect(DbContext, userId, seasonId)
            });
        }

        public IActionResult Error()
        {
            return View("~/Views/Shared/Error.cshtml");
        }

        #endregion

    }

}
