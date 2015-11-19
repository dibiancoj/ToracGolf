using Microsoft.AspNet.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Antiforgery;
using Microsoft.AspNet.Mvc.Filters;
using Microsoft.Extensions.Primitives;

namespace ToracGolf.Filters
{

    /// <summary>
    /// With angular we need a way to attach the validate anti token to the request and validate it.
    /// </summary>
    public class ValidateCustomAntiForgeryTokenAttribute : ActionFilterAttribute
    {

        //parts that contains this. 
        /*
        1. This action filter attribute
        ---------------------------------------------------
        2. CourseAdd - View = 
            @inject Microsoft.AspNet.Antiforgery.IAntiforgery Xsrf
            @functions
            {
                public string GetAntiXsrfToken()
                    {
                        var tokens = Xsrf.GetTokens(Context);
                        return tokens.CookieToken + ":" + tokens.FormToken;
                    }
                }

            <form....>
             <input type="hidden" name="__RequestVerificationToken" value="@GetAntiXsrfToken()" />

           ---------------------------------------------------
            3. Angular controller
              var token = $('[name=__RequestVerificationToken]').val();

                var config = {
                    headers: {
                        'RequestVerificationToken': token
                    }
                };

                //$scope.model 

                $http.post('AddACourse', $scope.model, config)
                   .then(function (response) {
                       debugger;
                       var s = response;

                       $scope.model.Location = 'teststest';

                       // this callback will be called asynchronously
                       // when the response is available
                   }, function (response) {
                       var s = response;
                       // called asynchronously if an error occurs
                       // or server returns response with an error status.
                   });

            4. Mvc Controller
              [ValidateCustomAntiForgeryToken()]
    */

        public override void OnActionExecuting(ActionExecutingContext actionContext)
        {
            IAntiforgery antiforgery = actionContext.HttpContext.ApplicationServices.GetService(typeof(IAntiforgery)) as IAntiforgery;

            var cookieToken = string.Empty;
            var formToken = string.Empty;
            StringValues tokenHeaders;
            string[] tokens = null;

            //go grab this token
            if (actionContext.HttpContext.Request.Headers.TryGetValue("RequestVerificationToken", out tokenHeaders))
            {
                tokens = tokenHeaders.First().Split(':');

                if (tokens != null && tokens.Length == 2)
                {
                    cookieToken = tokens[0];
                    formToken = tokens[1];
                }
                else
                {
                    throw new Exception("Can't Find Request Verification Token");
                }
            }
            else
            {
                throw new Exception("Can't Find Request Verification Token");
            }


            antiforgery.ValidateTokens(actionContext.HttpContext, new AntiforgeryTokenSet(formToken, cookieToken));

            base.OnActionExecuting(actionContext);
        }

    }

}
