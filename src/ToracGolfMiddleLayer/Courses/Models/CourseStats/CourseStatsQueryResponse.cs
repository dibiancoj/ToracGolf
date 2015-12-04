using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToracGolf.MiddleLayer.Dashboard.Models;

namespace ToracGolf.MiddleLayer.Courses.Models.CourseStats
{

    public class CourseStatsQueryResponse
    {
        public CondensedStats QuickStats { get; set; }
        //public IEnumerable<TeeBoxData> TeeBoxInfo { get; set; }

        public IEnumerable<DashboardHandicapScoreSplitDisplay> ScoreGraphData { get; set; }

        public IEnumerable<PuttsCourseStats> PuttsGraphData { get; set; }
    }

    public class CondensedStats
    {
        public int TeeBoxCount { get; set; }
        public int RoundCount { get; set; }
        public int BestScore { get; set; }
        public double AverageScore { get; set; }
    }

    public class TeeBoxData
    {
        public int TeeLocationId { get; set; }
        public string Name { get; set; }
        public int Yardage { get; set; }
        public int Par { get; set; }
        public double Slope { get; set; }
        public double Rating { get; set; }
    }

}
