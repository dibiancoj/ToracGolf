using Microsoft.AspNet.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ToracGolf.ViewComponents
{

    [ViewComponent(Name = "TopNavProfilePicture")]
    public class TopNavMenuViewComponent : ViewComponent
    {

        public IViewComponentResult Invoke()
        {
            var image = HttpContext.User.Claims.FirstOrDefault(x => x.Type == Constants.SecuritySettings.UserImageClaimUrl);

            if (image == null || string.IsNullOrEmpty(image.Value))
            {
                return Content(string.Empty);
            }

            return View("_TopNavUserProfilePicture", image.Value);
        }

    }

}
