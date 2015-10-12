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
        public int CoursePar { get; set; }
        public double HandicapBeforeRound { get; set; }
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

            if (difference > 7)
            {
                return RoundPerformanceEnum.AboveAverage;
            }

            if (difference < 3 && difference > -3)
            {
                return RoundPerformanceEnum.Average;
            }

            if (difference < -7)
            {
                return RoundPerformanceEnum.BadAverage;
            }

            return RoundPerformanceEnum.Awful;
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
