using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ToracGolf.MiddleLayer.HandicapCalculator.Models
{
    public class HandicapCalculateSeasonAndCareerModel
    {

        public Last20Rounds[] SeasonRounds { get; set; }
        public Last20Rounds[] CareerRounds { get; set; }

    }
}
