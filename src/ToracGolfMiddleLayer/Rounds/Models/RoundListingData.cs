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
        public int SeasonId { get; set; }
        public double RoundHandicap { get; set; }
        public int AdjustedScore { get; set; }
        public double HandicapBeforeRound { get; set; }

        public int? Putts { get; set; }
        public int? GreensInRegulation { get; set; }
        public int? FairwaysHit { get; set; }
        public int? FairwaysHitAttempted
        {
            get
            {
                return TeeBoxLocation.FairwaysOnCourse;
            }
        }
    }

    public class RoundPerformance
    {

        public static RoundPerformanceEnum CalculateRoundPerformance(int coursePar, int roundAdjustedScore)
        {
            var difference = coursePar - roundAdjustedScore;

            if (difference > 13)
            {
                return RoundPerformanceEnum.Awesome;
            }

            if (difference >= 4)
            {
                return RoundPerformanceEnum.AboveAverage;
            }

            if (difference <= 3 && difference >= -3)
            {
                return RoundPerformanceEnum.Average;
            }

            if (difference < -10)
            {
                return RoundPerformanceEnum.Awful;
            }

            if (difference < -4)
            {
                return RoundPerformanceEnum.BadAverage;
            }

            throw new NotImplementedException();
        }

        public enum RoundPerformanceEnum
        {
            Awful = 0,
            Bad = 1,
            BadAverage = 2,
            Average = 3,
            AboveAverage = 4,
            Awesome = 5
        }
    }

}
