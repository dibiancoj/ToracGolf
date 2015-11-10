using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using ToracGolf.MiddleLayer.CustomValidators;

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
        [Range(1, int.MaxValue, ErrorMessage = "Please Enter A Score Above 0")]
        [Required(ErrorMessage = "Score Is A Required Field")]
        public int? Score { get; set; }

        [Display(Name = "9 Hole Score")]
        [Required()]
        public bool NineHoleScore { get; set; }

        /// <summary>
        /// Just need this for asp.net binding when building the drop down
        /// </summary>
        public string StateId { get; set; }

        [Display(Name = "Greens In Regulation")]
        public int? GreensInRegulation { get; set; }

        [Display(Name = "Fairways Hit")]
        [FairwaysHitRequired(nameof(FairwaysHitPossible), ErrorMessage = "Number Of Fairways Is Required When Fairways Hit Is Filled Out")]
        public int? FairwaysHit { get; set; }

        [Display(Name = "Number Of Fairways")]
        public int? FairwaysHitPossible { get; set; }

        [Display(Name = "Number Of Putts")]
        public int? Putts { get; set; }

        #endregion

    }

}
