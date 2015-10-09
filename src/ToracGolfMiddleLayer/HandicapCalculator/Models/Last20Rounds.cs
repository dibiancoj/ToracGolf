using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ToracGolf.MiddleLayer.HandicapCalculator.Models
{

    public class Last20Rounds
    {

        #region Properties

        public int RoundId { get; set; }
        public double Slope { get; set; }
        public double Rating { get; set; }
        public int RoundScore { get; set; }

        #endregion

    }

}
