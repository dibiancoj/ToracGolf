using Microsoft.AspNet.Antiforgery;
using Microsoft.AspNet.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ToracGolf.Constants;

namespace ToracGolf.Controllers
{

    public class BaseController : Controller
    {

        protected const int UniqueConstraintId = 2627;

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

    }

}
