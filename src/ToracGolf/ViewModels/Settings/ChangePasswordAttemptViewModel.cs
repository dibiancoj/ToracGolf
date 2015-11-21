using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ToracGolf.ViewModels.Settings
{
    public class ChangePasswordAttemptViewModel
    {

        [DataType(DataType.Password)]
        [MaxLength(100)]
        [Required(ErrorMessage = "Old Password Is Required")]
        public string CurrentPW { get; set; }

        [DataType(DataType.Password)]
        [MaxLength(100)]
        [Required(ErrorMessage = "New Password 1 Is Required")]
        public string NewPw1 { get; set; }

        [DataType(DataType.Password)]
        [MaxLength(100)]
        [Compare("NewPw1", ErrorMessage = "Both New Password Fields Must Match")]
        [Required(ErrorMessage = "New Password 2 Is Required")]
        public string NewPw2 { get; set; }

    }
}
