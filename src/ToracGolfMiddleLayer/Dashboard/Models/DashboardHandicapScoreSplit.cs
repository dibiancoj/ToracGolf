using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ToracGolf.MiddleLayer.Dashboard.Models
{

    public class DashboardHandicapScoreSplitDisplay
    {

        public int Month { get; set; }
        public int Day { get; set; }
        public int Year { get; set; }

        public int Score { get; set; }
        public double Handicap { get; set; }

    }

}
