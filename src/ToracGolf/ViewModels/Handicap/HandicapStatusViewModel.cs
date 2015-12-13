using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ToracGolf.ViewModels.Handicap
{
    public class HandicapStatusViewModel
    {

        #region Constructor

        public HandicapStatusViewModel(double? seasonHandicap, double? careerHandicap, double? seasonProgression, double? careerProgression)
        {
            SeasonHandicap = seasonHandicap;
            CareerHandicap = careerHandicap;
            SeasonProgression = seasonProgression;
            CareerProgression = careerProgression;
        }

        #endregion

        #region Properties

        public double? SeasonHandicap { get; }
        public double? CareerHandicap { get; }
        public double? SeasonProgression { get; }
        public double? CareerProgression { get; }

        #endregion

    }
}
