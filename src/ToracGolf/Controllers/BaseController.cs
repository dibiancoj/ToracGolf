using Microsoft.AspNet.Antiforgery;
using Microsoft.AspNet.Http;
using Microsoft.AspNet.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ToracGolf.Constants;
using ToracGolf.MiddleLayer.EFModel;
using ToracGolf.ViewModels.Handicap;

namespace ToracGolf.Controllers
{

    public class BaseController : Controller
    {

        #region Constants

        protected const int UniqueConstraintId = 2627;
        private const string HandicapStatusSessionName = "HandicapStatusSession";

        #endregion

        #region Methods

        public bool IsUserAuthenticated()
        {
            return Context.User != null &&
               Context.User.Identity != null &&
               Context.User.Identity.IsAuthenticated;
        }

        public int GetUserId()
        {
            return Convert.ToInt32(Context.User.Claims.First(x => x.Type == ClaimTypes.Hash.ToString()).Value);
        }

        public HandicapStatusViewModel HandicapStatusBuilder(ToracGolfContext dbContext)
        {
            //if we have the object in session then use it!
            var attemptToGetHandicap = Context.Session.GetString(HandicapStatusSessionName);

            //did we find it?
            if (!string.IsNullOrEmpty(attemptToGetHandicap))
            {
                //we have it
                return JsonConvert.DeserializeObject<HandicapStatusViewModel>(attemptToGetHandicap);
            }

            //we need to go get the handicap... put his in business log
            //first thing we need to is go get the last 20 rounds (for season and career)
            var handicap = new HandicapStatusViewModel(10, 20);

           /finish logic for this...store this in the claim / Session, or quick lookup table maybe in user table? most likely in claim on user

            //set the session so we have it
            Context.Session.SetString(HandicapStatusSessionName, JsonConvert.SerializeObject(handicap));

            //return the handicap object
            return handicap;
        }

        public AntiforgeryTokenSet BuildTokenSet(IAntiforgery forgery)
        {
            //grab the token
            var token = forgery.GetAndStoreTokens(Context);

            //grab the cookie now
            var cookie = Context.Request.Cookies[SecuritySettings.AntiforgeryCookieName];

            //if the cookie is not blank and the token cookie is blank, then set it
            if (string.IsNullOrEmpty(token.CookieToken) && !string.IsNullOrEmpty(cookie))
            {
                return new AntiforgeryTokenSet(token.FormToken, cookie);
            }

            //else just return whatever we got back
            return token;
        }

        #endregion

    }

}
