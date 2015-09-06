﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using ToracGolf.ViewModels.Navigation;
using ToracGolf.ViewModels.Security;
using Microsoft.AspNet.Authorization;
using ToracGolf.Models;
using Microsoft.AspNet.Identity;
using System.Collections.Immutable;
using Microsoft.Framework.Caching.Memory;
using Microsoft.AspNet.Mvc.Rendering;

namespace ToracGolf.Controllers
{

    [Authorize]
    public class SecurityController : Controller
    {

        #region Constructor

        public SecurityController(SignInManager<ApplicationUser> signInManagerAPI, IMemoryCache cache)
        {
            SignInManagerAPI = signInManagerAPI;
            Cache = cache;
        }

        #endregion

        #region Properties

        private SignInManager<ApplicationUser> SignInManagerAPI { get; }

        private IMemoryCache Cache { get; }

        #endregion

        #region Methods

        #region Log In

        [HttpGet]
        [AllowAnonymous]
        [Route("LogIn")]
        public IActionResult LogIn()
        {
            var breadCrumb = new List<BreadcrumbNavItem>();

            breadCrumb.Add(new BreadcrumbNavItem("Home", "#"));
            breadCrumb.Add(new BreadcrumbNavItem("Sign In", "#"));

            return View(new LogInViewModel(breadCrumb));
        }

        //[HttpPost]
        //[AllowAnonymous]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
        //{
        //    ViewData["ReturnUrl"] = returnUrl;

        //    if (ModelState.IsValid)
        //    {
        //        // This doesn't count login failures towards account lockout
        //        // To enable password failures to trigger account lockout, set lockoutOnFailure: true
        //        var result = await SignInManagerAPI.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);
        //        if (result.Succeeded)
        //        {
        //            return RedirectToLocal(returnUrl);
        //        }
        //        if (result.RequiresTwoFactor)
        //        {
        //            return RedirectToAction(nameof(SendCode), new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
        //        }
        //        if (result.IsLockedOut)
        //        {
        //            return View("Lockout");
        //        }
        //        else
        //        {
        //            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
        //            return View(model);
        //        }
        //    }

        //    // If we got this far, something failed, redisplay form
        //    return View(model);
        //}

        #endregion

        #endregion

        #region Sign Up

        [HttpGet]
        [AllowAnonymous]
        [Route("SignUp")]
        public IActionResult SignUp()
        {
            var breadCrumb = new List<BreadcrumbNavItem>();

            breadCrumb.Add(new BreadcrumbNavItem("Home", "#"));
            breadCrumb.Add(new BreadcrumbNavItem("Sign Up", "#"));

            //TODO: should implement caching code.

            IImmutableDictionary<string, string> states;

#if DNX451
            // utilize resource only available with .NET Framework
            states = ToracLibrary.Core.States.State.UnitedStatesStateListing();
#else
            //todo: do i need to add a .net core thing here?
            states = new Dictionary<string,string>().ToImmutableDictionary();
#endif

            return View(new SignUpInViewModel(breadCrumb, states.Select(x => new SelectListItem { Text = x.Value, Value = x.Key })
                                                                .OrderBy(x => x.Text).ToArray()));
        }

        #endregion

    }
}
