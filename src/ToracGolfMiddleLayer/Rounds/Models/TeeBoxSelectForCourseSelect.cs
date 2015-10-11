using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToracGolf.MiddleLayer.EFModel.Tables;

namespace ToracGolf.MiddleLayer.Rounds.Models
{
    public class TeeBoxSelectForCourseSelect
    {

     
        public int CourseTeeLocationId { get; set; }

     
        public int CourseId { get; set; }

     
        public string Description { get; set; }

      
        public int TeeLocationSortOrderId { get; set; }

       
        public int Yardage { get; set; }

    
        public int Front9Par { get; set; }

       
        public int Back9Par { get; set; }

      
        public double Rating { get; set; }

       
        public double Slope { get; set; }

        public double? CourseTeeBoxHandicap { get; set; }

        public string MaxScorePerHole { get; set; }

    }
}
