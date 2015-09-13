using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Authorization;

namespace ToracGolf.Controllers
{

    [Authorize]
    public class SettingsController : Controller
    {

        [HttpGet]
        [AllowAnonymous]
        [Route("ChangeMySettings", Name = "ChangeMySettings")]      
        public IActionResult Index()
        {
            return View();
        }
    }

}
