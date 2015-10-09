using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToracGolf.MiddleLayer.EFModel.Tables;

namespace ToracGolf.MiddleLayer.Rounds.Models
{

    public class RoundSelectModel
    {

        public RoundSelectModel(IDictionary<int, byte[]> courseImages, IEnumerable<RoundListingData> listingData)
        {
            CourseImages = courseImages;
            ListingData = listingData;
        }

        public IDictionary<int, byte[]> CourseImages { get; }
        public IEnumerable<RoundListingData> ListingData { get; }
    }

    public class RoundListingData
    {
        public int RoundId { get; set; }
        public int CourseId { get; set; }
        public string CourseName { get; set; }
        public int Score { get; set; }
        public CourseTeeLocations TeeBoxLocation { get; set; }
        public DateTime RoundDate { get; set; }
        public int RoundPerformance { get; set; }
    }

    public enum RoundPerformance
    {
        Awful = 0,
        Bad = 1,
        BadAverage = 2,
        Average = 3,
        AboveAverage = 4,
        Awesome = 5
    }

}
