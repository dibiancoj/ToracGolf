using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
        public DateTime RoundDate { get; set; }
    }

}
