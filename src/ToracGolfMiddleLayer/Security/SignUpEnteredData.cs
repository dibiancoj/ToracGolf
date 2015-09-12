using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ToracGolf.MiddleLayer.Security
{

    public class SignUpEnteredDataBase
    {

        #region Model Properties

        [Display(Name = "Email")]
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Display(Name = "First Name")]
        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        [Required]
        [MaxLength(50)]
        public string LastName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [MaxLength(100)]
        public string Password { get; set; }

        [Display(Name = "Current Season")]
        [Required]
        [MaxLength(50)]
        public string CurrentSeason { get; set; }

        [Display(Name = "Home State")]
        [Required]
        public string HomeState { get; set; }

        #endregion


    }

    public class SignUpEnteredData : SignUpEnteredDataBase
    {

        [Display(Name = "Re-Enter Your Password")]
        [Compare("Password")]
        [Required]
        [DataType(DataType.Password)]
        [MaxLength(100)]
        public string PasswordReenter { get; set; }

    }

}
