﻿using System;
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

        public IEnumerable<PuttsCourseStatsGraph> PuttsGraphData { get; set; }

        public IEnumerable<FairwaysInRegulationCourseStatsGraph> FairwaysGraphData { get; set; }

        public IEnumerable<GreensInRegulationCourseStatsGraph> GIRGraphData { get; set; }

        public IEnumerable<RecentCourseStatsRounds> RecentRounds { get; set; }
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

    public class RecentCourseStatsRounds
    {
        public int RoundId { get; set; }
        public string TeeBoxLocation { get; set; }
        public DateTime RoundDate { get; set; }
        public int Score { get; set; }
    }

}
