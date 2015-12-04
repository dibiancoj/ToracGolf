using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToracGolf.MiddleLayer.Dashboard.Models;

namespace ToracGolf.MiddleLayer.Courses.Models.CourseStats
{
    public class PuttsCourseStats : ChartDateTimeMonthBase
    {
        public int Putts { get; set; }
    }
}
