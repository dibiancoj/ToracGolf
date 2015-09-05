using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;

namespace ToracGolf.Controllers
{
    public class SecurityController : Controller
    {
        
        [Route("LogIn")]
        public IActionResult LogIn()
        {
            return View();
        }

    }
}
