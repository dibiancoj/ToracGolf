using Microsoft.AspNet.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ToracGolf.Controllers
{

    public class BaseController : Controller
    {

        public bool IsUserAuthenticated()
        {
            return Context.User != null &&
               Context.User.Identity != null &&
               Context.User.Identity.IsAuthenticated;
        }

    }

}
