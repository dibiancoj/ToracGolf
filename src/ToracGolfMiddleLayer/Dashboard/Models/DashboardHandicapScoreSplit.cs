﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ToracGolf.MiddleLayer.Dashboard.Models
{

    public class DashboardHandicapScoreSplitDisplay : ChartDateTimeMonthBase
    {

        public int Score { get; set; }
        public double Handicap { get; set; }

    }

}
