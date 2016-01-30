using Microsoft.AspNet.Antiforgery;
using Microsoft.AspNet.Http;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Mvc.Rendering;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ToracGolf.Constants;
using ToracGolf.MiddleLayer.EFModel;
using ToracGolf.MiddleLayer.HandicapCalculator;
using ToracGolf.MiddleLayer.Season;
using ToracGolf.ViewModels.Handicap;

namespace ToracGolf.Controllers
{

    public class BaseController : Controller
    {

        #region Constants

        protected const int UniqueConstraintId = 2627;
        protected const string HandicapStatusSessionName = "HandicapStatusSession";
        protected const string UserCurrentSeasonSessionName = "CurrentSeasonSession";

        #endregion

        #region Methods

        #region User Information

        public bool IsUserAuthenticated()
        {
            return HttpContext.User != null &&
              HttpContext.User.Identity != null &&
              HttpContext.User.Identity.IsAuthenticated;
        }

        public async Task<int> UserCurrentSeason(ToracGolfContext dbContext, int userId)
        {
            //we will store this in session for performance
            var attemptToGetCurrentSeasonId = HttpContext.Session.GetInt32(UserCurrentSeasonSessionName);

            //do we have a value?
            if (attemptToGetCurrentSeasonId.HasValue)
            {
                //we have a current season, return it
                return attemptToGetCurrentSeasonId.Value;
            }

            //we don't have it in session, go to the database to grab it
            var currentSeasonId = await SeasonDataProvider.CurrentSeasonForUser(dbContext, userId);

            //now put it in session
            HttpContext.Session.SetInt32(UserCurrentSeasonSessionName, currentSeasonId);

            //now return the current season id
            return currentSeasonId;
        }

        public int GetUserId()
        {
            return Convert.ToInt32(HttpContext.User.Claims.First(x => x.Type == ClaimTypes.Hash.ToString()).Value);
        }

        public string GetUserDefaultState()
        {
            return HttpContext.User.Claims.First(x => x.Type == ClaimTypes.StateOrProvince).Value;
        }

        #endregion

        #region Handicap

        public async Task<HandicapStatusViewModel> HandicapStatusBuilder(ToracGolfContext dbContext, int userId, int userCurrentSeason)
        {
            //if we have the object in session then use it!
            var attemptToGetHandicap = HttpContext.Session.GetString(HandicapStatusSessionName);

            //did we find it?
            if (!string.IsNullOrEmpty(attemptToGetHandicap))
            {
                //we have it
                return JsonConvert.DeserializeObject<HandicapStatusViewModel>(attemptToGetHandicap);
            }

            //we need to go get the handicap... put his in business log
            //first thing we need to is go get the last 20 rounds (for season and career)

            //let's go grab the season data
            var tskSeasonRounds = await HandicapDataProvider.HandicapCalculatorRoundSelectorSeason(dbContext, userId, userCurrentSeason).ContinueWith(tsk =>
            {
                return Handicapper.CalculateHandicap(tsk.Result);
            });

            //go grab the career
            var tskCareerRounds = await HandicapDataProvider.HandicapCalculatorRoundSelectorCareer(dbContext, userId).ContinueWith(tsk =>
             {
                 return Handicapper.CalculateHandicap(tsk.Result);
             });

            //grab the season progression
            var seasonProgression = await HandicapDataProvider.HandicapProgression(dbContext, userId, userCurrentSeason);

            //grab the career progression
            var careerProgression = await HandicapDataProvider.HandicapProgression(dbContext, userId, null);

            //go wait for both of them and build up our handicap model
            var handicap = new HandicapStatusViewModel(tskSeasonRounds, tskCareerRounds, Handicapper.HandicapProgression(seasonProgression), Handicapper.HandicapProgression(careerProgression));

            //set the session so we have it
            HttpContext.Session.SetString(HandicapStatusSessionName, JsonConvert.SerializeObject(handicap));

            //return the handicap object
            return handicap;
        }

        #endregion

        #region Security

        public AntiforgeryTokenSet BuildTokenSet(IAntiforgery forgery)
        {
            //grab the token
            var token = forgery.GetAndStoreTokens(HttpContext);

            //grab the cookie now
            var cookie = HttpContext.Request.Cookies[SecuritySettings.AntiforgeryCookieName];

            //if the cookie is not blank and the token cookie is blank, then set it
            if (string.IsNullOrEmpty(token.CookieToken) && !string.IsNullOrEmpty(cookie))
            {
                return new AntiforgeryTokenSet(token.FormToken, cookie);
            }

            //else just return whatever we got back
            return token;
        }

        #endregion

        #region Select List Items

        public IEnumerable<SelectListItem> BuildSelectList<T>(IEnumerable<T> Collection, Func<T, string> IdSelector, Func<T, string> DescriptionSelector, Func<SelectListItem> BlankSelector)
        {
            //build the base list
            var lst = Collection.Select(x => new SelectListItem { Value = IdSelector(x), Text = DescriptionSelector(x) }).ToList();

            if (BlankSelector != null)
            {
                //add the "all seasons"
                lst.Insert(0, BlankSelector());
            }

            return lst;
        }

        #endregion

        #endregion

    }

}
