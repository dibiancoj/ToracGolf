using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ToracGolf.MiddleLayer.HandicapCalculator.Models
{

    public class Last20Rounds
    {

        #region Constructor

        public Last20Rounds(int roundId,float scope, float rating, int roundScore)
        {
            RoundId = roundId;
            Scope = scope;
            Rating = rating;
            RoundScore = roundScore;
        }

        #endregion

        #region Properties

        public int RoundId { get; }
        public float Scope { get; }
        public float Rating { get; }
        public int RoundScore { get; }

        #endregion

    }

}
