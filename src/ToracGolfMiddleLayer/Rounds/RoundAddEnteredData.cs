using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ToracGolf.MiddleLayer.Rounds
{

    public class RoundAddEnteredData
    {

        #region Model Properties

        [Display(Name = "Round Date")]
        [Required(ErrorMessage = "Round Date Is A Required Field")]
        public DateTime RoundDate { get; set; }

        [Display(Name = "Course")]
        [Required(ErrorMessage = "A Course Is A Required Field")]
        public int CourseId { get; set; }

        [Display(Name = "Tee Location")]
        [Required(ErrorMessage = "A Tee Box Is A Required Field")]
        public int TeeLocationId { get; set; }

        [Display(Name = "Score")]
        [Required(ErrorMessage = "Score Is A Required Field")]
        public int Score { get; set; }

        [Display(Name = "9 Hole Score")]
        [Required()]
        public bool NineHoleScore { get; set; }

        /// <summary>
        /// Just need this for asp.net binding when building the drop down
        /// </summary>
        public string StateId { get; set; }

        #endregion

    }

}
