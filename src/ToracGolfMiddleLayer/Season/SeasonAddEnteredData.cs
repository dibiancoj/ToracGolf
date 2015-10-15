using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ToracGolf.MiddleLayer.Season
{

    public class SeasonAddEnteredData
    {

        #region Model Properties

        [Display(Name = "Season Description")]
        [Required(ErrorMessage = "Season Description Is Required")]
        [MaxLength(50)]
        public string SeasonDescription { get; set; }

        [Display(Name = "Make This Your Current Season?")]
        public bool MakeCurrentSeason { get; set; }

        #endregion

    }

}
