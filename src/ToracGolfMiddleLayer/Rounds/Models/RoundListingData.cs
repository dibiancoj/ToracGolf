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
    }

    public class RoundPerformance
    {

        public static RoundPerformanceEnum CalculateRoundPerformance(int coursePar, int roundAdjustedScore)
        {
            if (!currentHandicap.HasValue)
            {
                return RoundPerformanceEnum.Average;
            }

            //calculate my handicap minus my round handicap
            var differenceInHandicaps = currentHandicap - roundHandicap;

            //what level are we at?
            if (differenceInHandicaps > 10)
            {
                return RoundPerformanceEnum.Awesome;
            }
            else if (differenceInHandicaps > 5)
            {
                return RoundPerformanceEnum.AboveAverage;
            }
            else if (differenceInHandicaps >= -5 && differenceInHandicaps <= 5)
            {
                return RoundPerformanceEnum.Average;
            }
            else if (differenceInHandicaps < -10)
            {
                return RoundPerformanceEnum.Awful;
            }
            else if (differenceInHandicaps < -5)
            {
                return RoundPerformanceEnum.BadAverage;
            }
            else
            {
                return RoundPerformanceEnum.Average;
            }
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
