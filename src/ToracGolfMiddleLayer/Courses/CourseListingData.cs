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

        public int TeeLocationCount { get; set; }

        public CourseImages CourseImage { get; set; }

        #endregion

    }

}
