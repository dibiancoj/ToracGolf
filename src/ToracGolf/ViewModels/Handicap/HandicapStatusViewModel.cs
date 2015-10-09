using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ToracGolf.ViewModels.Handicap
{
    public class HandicapStatusViewModel
    {

        #region Constructor

        public HandicapStatusViewModel(double? seasonHandicap, double? careerHandicap)
        {
            SeasonHandicap = seasonHandicap;
            CareerHandicap = careerHandicap;
        }

        #endregion

        #region Properties

        public double? SeasonHandicap { get; }
        public double? CareerHandicap { get; }

        #endregion

    }
}
