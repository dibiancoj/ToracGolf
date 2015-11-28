using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ToracGolf.MiddleLayer.Courses.Models.CourseStats
{
    public class CourseStatsQueryRequest
    {
        public int CourseId { get; set; }
        public int SeasonId { get; set; }
        public int TeeBoxLocationId { get; set; }

    }
}
