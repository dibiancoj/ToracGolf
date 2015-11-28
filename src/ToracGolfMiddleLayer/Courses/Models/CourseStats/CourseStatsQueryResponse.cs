using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ToracGolf.MiddleLayer.Courses.Models.CourseStats
{

    public class CourseStatsQueryResponse
    {
        public CondensedStats QuickStats { get; set; }
    }

    public class CondensedStats
    {
        public int TeeBoxCount { get; set; }
        public int RoundCount { get; set; }
        public int BestScore { get; set; }
        public double AverageScore { get; set; }
    }

}
