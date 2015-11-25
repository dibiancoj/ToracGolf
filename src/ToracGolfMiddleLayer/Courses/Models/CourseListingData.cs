using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToracGolf.MiddleLayer.EFModel.Tables;

namespace ToracGolf.MiddleLayer.Courses
{
    public class CourseListingData
    {

        #region Properties

        public Course CourseData { get; set; }

        public string StateDescription { get; set; }

        public int TeeLocationCount { get; set; }

        public CourseImages CourseImage { get; set; }

        public int NumberOfRounds { get; set; }
        public int? TopScore { get; set; }
        public int? WorseScore { get; set; }
        public double? AverageScore { get; set; }

        public double? GreensInRegulation { get; set; }
        public int? FairwaysHit { get; set; }
        public int? FairwaysHitAttempted { get; set; }
        public double? NumberOfPutts { get; set; }

        #endregion

    }

}
