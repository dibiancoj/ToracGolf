using Microsoft.AspNet.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

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

    }

}
