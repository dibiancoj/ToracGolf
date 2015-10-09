using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ToracGolf.ViewModels.Handicap
{
    public class HandicapStatusViewModel
    {

        #region Constructor

        public HandicapStatusViewModel(float seasonHandicap, float careerHandicap)
        {
            SeasonHandicap = seasonHandicap;
            CareerHandicap = careerHandicap;
        }

        #endregion

        #region Properties

        public float SeasonHandicap { get; }
        public float CareerHandicap { get; }

        #endregion

    }
}
