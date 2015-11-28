using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToracGolf.MiddleLayer.Common;
using ToracGolf.MiddleLayer.EFModel.Tables;

namespace ToracGolf.MiddleLayer.Courses.Models
{
    public class CourseStatsModel
    {
        public Course CourseData { get; set; }

        public int TeeBoxCount
        {
            get
            {
                return TeeBoxLocations.Count();
            }
        }

        public int RoundCount { get; set; }

        public string CourseState { get; set; }

        public byte[] CourseImage { get; set; }

        public int BestScore { get; set; }

        public double AverageScore { get; set; }

        public IEnumerable<EFKeyValuePair> TeeBoxLocations { get; set; }
    }
}
