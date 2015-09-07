using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ToracGolf.ViewModels.Security
{
    public class LogInViewModel
    {

        public LogInEnteredData LogInUserEntered { get; set; }

        public IList<Navigation.BreadcrumbNavItem> Breadcrumb { get; set; }
    }

    public class LogInEnteredData
    {

        #region Model Properties

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }

        #endregion

    }

}
