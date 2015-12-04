using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToracGolf.MiddleLayer.Dashboard.Models;

namespace ToracGolf.MiddleLayer.Courses.Models.CourseStats
{
    public class PuttsCourseStatsGraph : ChartDateTimeMonthBase
    {
        public int Putts { get; set; }
    }

    public class GreensInRegulationCourseStatsGraph : ChartDateTimeMonthBase
    {
        public int GreensHit { get; set; }
    }

    public class FairwaysInRegulationCourseStatsGraph : ChartDateTimeMonthBase
    {
        public int FairwaysHit { get; set; }
    }
}
