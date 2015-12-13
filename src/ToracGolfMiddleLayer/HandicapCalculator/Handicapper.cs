using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToracGolf.MiddleLayer.HandicapCalculator.Models;

namespace ToracGolf.MiddleLayer.HandicapCalculator
{

    public static class Handicapper
    {

        #region Constants

        private const int CourseHandicapFactor = 113;

        #endregion

        #region Public Methods

        /// <summary>
        /// Calculate a handicap
        /// </summary>
        /// <param name="Last20Rounds">Last 20 rounds to calculate. If the player hasn't played 20, whatever they have</param>
        /// <returns>The handicap. Null if we don't have any rounds</returns>
        public static double? CalculateHandicap(ICollection<Last20Rounds> Last20RoundsToCalculate)
        {
            //validate there isn't more then 20 rounds
            if (Last20RoundsToCalculate.Count > 20)
            {
                throw new ArgumentOutOfRangeException(nameof(Last20Rounds), "Only 20 Items Allowed In The Last20Round Collection");
            }

            if (!Last20RoundsToCalculate.Any())
            {
                //we don't have any rounds just return null
                return null;
            }

            //validation is done...

            //we need to calculate the differential (so loop through each round and calc the differential)
            var sortedDifferential = (from myData in Last20RoundsToCalculate
                                      select new
                                      {
                                          Differential = CalculateDifferential(myData.RoundScore, myData.Rating, myData.Slope),
                                          RoundRecord = myData
                                      }).OrderBy(x => x.Differential).ToArray();

            //how many rounds are we using to calculate this thing with?
            int howManyRoundToUse = HowManyRoundsToUseInFormula(Last20RoundsToCalculate.Count);

            //now sum the differential for x amount of rounds that we have above
            double sumOfDifferential = sortedDifferential.Take(howManyRoundToUse).Sum(x => x.Differential);

            //now grab the avg
            double averageDifferential = sumOfDifferential / howManyRoundToUse;

            //add the special clause
            return Math.Round(averageDifferential * .96, 1);
        }

        /// <summary>
        /// Calculate a course handicap. This way you can grab the max score on a hole
        /// </summary>
        /// <returns>course handicap</returns>
        public static double? CalculateCourseHandicap(double? currentHandicap, double slopeOfTeeBox)
        {
            //make sure we have a handicap first
            if (!currentHandicap.HasValue)
            {
                return null;
            }

            //else let's run the formula
            return Math.Round((currentHandicap.Value * slopeOfTeeBox) / CourseHandicapFactor, 1);
        }

        /// <summary>
        /// Calculate the max hold per hold
        /// </summary>
        public static string MaxScorePerHole(double? courseHandicap)
        {
            //if we don't have a course handicap (because we don't have a handicap)
            if (!courseHandicap.HasValue || courseHandicap.Value >= 39)
            {
                return "10";
            }

            if (courseHandicap.Value <= 9)
            {
                return "Double Bogey";
            }

            if (courseHandicap.Value >= 9 && courseHandicap.Value < 19)
            {
                return "7";
            }

            if (courseHandicap.Value >= 19 && courseHandicap.Value < 29)
            {
                return "8";
            }

            if (courseHandicap.Value >= 29 && courseHandicap.Value < 39)
            {
                return "9";
            }

            throw new NotImplementedException();
        }

        public static double RoundHandicap(int score, double teeBoxRating, double teeBoxSlope)
        {
            return CalculateDifferential(score, teeBoxRating, teeBoxSlope);

            //go calc the diff
            //var differential = CalculateDifferential(score, teeBoxRating, teeBoxSlope);

            ////now add the factory
            //return Math.Round(differential * .96, 1);
        }

        public static int HowManyRoundsToUseInFormula(int howManyRoundsToCalculateWith)
        {
            switch (howManyRoundsToCalculateWith)
            {
                case 1:
                case 2:
                case 3:
                case 4:
                case 5:
                case 6:
                    return 1;

                case 7:
                case 8:
                    return 2;

                case 9:
                case 10:
                    return 3;

                case 11:
                case 12:
                    return 4;

                case 13:
                case 14:
                    return 5;

                case 15:
                case 16:
                    return 6;

                case 17:
                    return 7;

                case 18:
                    return 8;

                case 19:
                    return 9;

                case 20:
                    return 10;

                default:
                    throw new ArgumentOutOfRangeException(nameof(howManyRoundsToCalculateWith), "You should only have 20 rounds at most to calculate");
            }
        }

        #region Handicap Progression

        public static double? HandicapProgression(HandicapProgression handicapProgressionRecords)
        {
            if (handicapProgressionRecords == null)
            {
                return null;
            }

            return handicapProgressionRecords.HandicapAfterRound - handicapProgressionRecords.HandicapBeforeRound;
        }

        #endregion

        #endregion

        #region Helper Methods

        private static double CalculateDifferential(int score, double teeBoxRating, double teeBoxSlope)
        {
            return ((score - teeBoxRating) * CourseHandicapFactor / teeBoxSlope);
        }

        #endregion

    }

}
