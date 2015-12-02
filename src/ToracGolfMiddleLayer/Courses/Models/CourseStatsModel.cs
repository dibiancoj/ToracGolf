using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToracGolf.MiddleLayer.Common;
using ToracGolf.MiddleLayer.Courses.Models.CourseStats;
using ToracGolf.MiddleLayer.EFModel.Tables;

namespace ToracGolf.MiddleLayer.Courses.Models
{
    public class CourseStatsModel
    {

        public int CourseId { get; set; }

        //public byte[] CourseImage { get; set; }

        public string CourseName { get; set; }

        public string CourseCity { get; set; }

        public string CourseState { get; set; }

        public string CourseDescription { get; set; }

        public string CourseImageUrl { get; set; }

        public IEnumerable<TeeBoxData> TeeBoxLocations { get; set; }

    }
}
