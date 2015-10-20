using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ToracGolf.MiddleLayer.Season.Models
{

    public class SeasonListingData
    {

        #region Properties

        public int SeasonId { get; set; }
        public string Description { get; set; }
        public int NumberOfRounds { get; set; }
        public int? TopScore { get; set; }
        public int? WorseScore { get; set; }
        public double? AverageScore { get; set; }

        #endregion

    }

}
