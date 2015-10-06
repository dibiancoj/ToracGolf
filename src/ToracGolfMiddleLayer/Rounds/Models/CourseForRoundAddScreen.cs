using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ToracGolf.MiddleLayer.Rounds.Models
{

    public class CourseForRoundAddScreen
    {

        public int CourseId { get; set; }
        public string Name { get; set; }
        public IEnumerable<TeeBoxForRoundAddScreen> TeeLocations { get; set; }

    }

    public class TeeBoxForRoundAddScreen
    {
        public int CourseTeeLocationId { get; set; }
        public string Description { get; set; }
        public int Yardage { get; set; }
        public int Front9Par { get; set; }
        public int Back9Par { get; set; }
        public int Par { get { return Front9Par + Back9Par; } }
        public double Rating { get; set; }
        public double Slope { get; set; }
    }

}
