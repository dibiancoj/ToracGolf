using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using ToracGolf.MiddleLayer.EFModel;
using ToracGolf.MiddleLayer.EFModel.Tables;
using ToracGolf.MiddleLayer.HandicapCalculator;
using ToracGolf.MiddleLayer.HandicapCalculator.Models;
using ToracGolf.MiddleLayer.Rounds.Models;
using ToracLibrary.AspNet.Paging;

namespace ToracGolf.MiddleLayer.Rounds
{
    public static class RoundDataProvider
    {

        #region General Lookups

        public static async Task<IEnumerable<CourseForRoundAddScreen>> CoursesSelectForState(ToracGolfContext dbContext, int stateId)
        {
            return await dbContext.Course.AsNoTracking()
                         .Where(x => x.StateId == stateId)
                       .Select(x => new CourseForRoundAddScreen
                       {
                           CourseId = x.CourseId,
                           Name = x.Name,
                       }).ToArrayAsync();
        }

        public static async Task<IEnumerable<TeeBoxSelectForCourseSelect>> TeeBoxSelectForCourse(ToracGolfContext dbContext, int courseId, double? currentHandicap)
        {
            var dbResults = await dbContext.CourseTeeLocations.AsNoTracking()
                                   .Where(x => x.CourseId == courseId).ToArrayAsync();


            var lst = new List<TeeBoxSelectForCourseSelect>();

            foreach (var teeBox in dbResults)
            {
                var teeBoxModel = new TeeBoxSelectForCourseSelect
                {
                    Back9Par = teeBox.Back9Par,
                    Front9Par = teeBox.Front9Par,
                    CourseId = teeBox.CourseId,
                    CourseTeeLocationId = teeBox.CourseTeeLocationId,
                    Description = teeBox.Description,
                    Rating = teeBox.Rating,
                    Slope = teeBox.Slope,
                    Yardage = teeBox.Yardage,
                    TeeLocationSortOrderId = teeBox.TeeLocationSortOrderId
                };

                //now add the course handicap and the max score
                teeBoxModel.CourseTeeBoxHandicap = HandicapCalculator.Handicapper.CalculateCourseHandicap(currentHandicap, teeBoxModel.Slope);

                //now calculate the score
                teeBoxModel.MaxScorePerHole = HandicapCalculator.Handicapper.MaxScorePerHole(teeBoxModel.CourseTeeBoxHandicap);

                //add the tee box score
                lst.Add(teeBoxModel);
            }

            //return the list
            return lst;
        }

        #endregion

        #region Round Add

        public static async Task<int> SaveRound(ToracGolfContext dbContext, int userId, int seasonId, RoundAddEnteredData roundData, double? currentHandicap)
        {
            //build the round record
            var round = new Round
            {
                CourseId = roundData.CourseId,
                CourseTeeLocationId = roundData.TeeLocationId,
                RoundDate = roundData.RoundDate,
                Is9HoleScore = roundData.NineHoleScore,
                Score = roundData.Score.Value,
                UserId = userId,
                SeasonId = seasonId
            };

            //add the round
            dbContext.Rounds.Add(round);

            //save the round
            //await dbContext.SaveChangesAsync();
            dbContext.SaveChanges();

            //we need to go add the handicap records

            //1. grab all the rounds after to this round (we need to delete them and recalculate them)
            var roundsAfterNewRound = await dbContext.Rounds
                                                       .Where(x => x.RoundDate > round.RoundDate)
                                                       .OrderBy(x => x.RoundDate).ThenBy(x => x.RoundId)
                                                       .Select(x => x.RoundId).ToArrayAsync();

            //2. if we have 0 rows, then just add a blank one with the current row
            if (!roundsAfterNewRound.Any())
            {
                //handicap to use to insert
                double handicapToUse;

                //do we have a handicap?
                if (currentHandicap.HasValue)
                {
                    //we already have a handicap, use it!
                    handicapToUse = currentHandicap.Value;
                }
                else
                {
                    //we don't have a handicap, we will use the inserted value for the handicap. 
                    //grab this tee box
                    var teeBox = await dbContext.CourseTeeLocations.FirstAsync(x => x.CourseId == round.CourseId && x.CourseTeeLocationId == round.CourseTeeLocationId);

                    //calculate the handicap for just this round
                    handicapToUse = Handicapper.CalculateHandicap(new Last20Rounds[]
                    {
                        new Last20Rounds { RoundId = round.RoundId, Rating = teeBox.Rating, Slope = teeBox.Slope, RoundScore = round.Score }
                    }).Value;
                }

                //add the record to the context
                dbContext.RoundHandicap.Add(new RoundHandicap { RoundId = round.RoundId, HandicapBeforeRound = handicapToUse });
            }
            else
            {
                //3. if we have rounds before the one we just inserted, we will go and delete everything and recalc. we can probably make this more eff.
                //but this is ok for now

                //4. go grab allt he
                dbContext.RoundHandicap.RemoveRange(await dbContext.RoundHandicap.Where(x => roundsAfterNewRound.Contains(x.RoundId)).ToArrayAsync());

                //save the changes
                await dbContext.SaveChangesAsync();

                //5. let's go rebuild all the handicap records
                var roundsForUser = await dbContext.Rounds.Where(x => x.UserId == userId)
                                    .OrderBy(x => x.RoundDate).ThenBy(x => x.RoundId)
                                    .Select(x => new
                                    {
                                        RoundRecord = x,
                                        TeeLocation = dbContext.CourseTeeLocations.FirstOrDefault(y => y.CourseTeeLocationId == x.CourseTeeLocationId && y.CourseId == x.CourseId)
                                    }).ToArrayAsync();

                //lets loop through those rounds for the user
                foreach (var roundToCalculate in roundsForUser)
                {
                    //grab the 20 rounds before this round
                    var roundsToUse = roundsForUser.Where(x => x.RoundRecord.RoundDate < roundToCalculate.RoundRecord.RoundDate)
                        .OrderByDescending(x => x.RoundRecord.RoundDate).ThenByDescending(x => x.RoundRecord.RoundId).Take(20).Select(x => new Last20Rounds
                        {
                            RoundId = x.RoundRecord.RoundId,
                            RoundScore = x.RoundRecord.Score,
                            Rating = x.TeeLocation.Rating,
                            Slope = x.TeeLocation.Slope
                        }).ToArray();

                    //if we have 0 rounds, then use the round we are looking through
                    if (!roundsToUse.Any())
                    {
                        roundsToUse = new Last20Rounds[] { new Last20Rounds
                        {
                            RoundId = roundToCalculate.RoundRecord.RoundId,
                            RoundScore = roundToCalculate.RoundRecord.Score,
                            Rating = roundToCalculate.TeeLocation.Rating,
                            Slope = roundToCalculate.TeeLocation.Slope
                        }};
                    }

                    //go calculate the handicap for this round (using the last 20 rounds)
                    var calculatedHandicap = Handicapper.CalculateHandicap(roundsToUse);

                    //add the record to the context
                    dbContext.RoundHandicap.Add(new RoundHandicap { RoundId = roundToCalculate.RoundRecord.RoundId, HandicapBeforeRound = calculatedHandicap.Value });
                }
            }

            //go save the changes for the course handicap
            await dbContext.SaveChangesAsync();

            //return the round id
            return round.RoundId;
        }

        #endregion

        #region Round Listing

        public static IQueryable<RoundListingData> RoundSelectQueryBuilder(ToracGolfContext dbContext, int userId, string courseNameFilter, int? seasonFilter)
        {
            //build the queryable
            var queryable = dbContext.Rounds.AsNoTracking().Where(x => x.UserId == userId).AsQueryable();

            //if we have a course name, add it as a filter
            if (!string.IsNullOrEmpty(courseNameFilter))
            {
                queryable = queryable.Where(x => x.Course.Name.Contains(courseNameFilter));
            }

            //do we have a state filter?
            if (seasonFilter.HasValue)
            {
                queryable = queryable.Where(x => x.SeasonId == seasonFilter.Value);
            }

            //go return the queryable
            return queryable.Select(x => new RoundListingData
            {
                RoundId = x.RoundId,
                CourseId = x.CourseId,
                CourseName = x.Course.Name,
                RoundDate = x.RoundDate,
                Score = x.Score,
                SeasonId = x.SeasonId,
                TeeBoxLocation = dbContext.CourseTeeLocations.FirstOrDefault(y => y.CourseId == x.CourseId && y.CourseTeeLocationId == x.CourseTeeLocationId),
                HandicapBeforeRound = dbContext.RoundHandicap.FirstOrDefault(y => y.RoundId == x.RoundId).HandicapBeforeRound
            });
        }

        /// <param name="pageId">0 base index that holds what page we are on</param>
        public static async Task<RoundSelectModel> RoundSelect(ToracGolfContext dbContext, int userId, int pageId, RoundListingSortOrder.RoundListingSortEnum sortBy, string courseNameFilter, int? seasonFilter, int recordsPerPage)
        {
            //how many items to skip
            int skipAmount = pageId * recordsPerPage;

            //go grab the query
            var queryable = RoundSelectQueryBuilder(dbContext, userId, courseNameFilter, seasonFilter);

            //figure out what you want to order by
            if (sortBy == RoundListingSortOrder.RoundListingSortEnum.CourseNameAscending)
            {
                queryable = queryable.OrderBy(x => x.CourseName);
            }
            else if (sortBy == RoundListingSortOrder.RoundListingSortEnum.CourseNameDescending)
            {
                queryable = queryable.OrderByDescending(x => x.CourseName);
            }
            else if (sortBy == RoundListingSortOrder.RoundListingSortEnum.RoundDateAscending)
            {
                queryable = queryable.OrderBy(x => x.RoundDate).ThenBy(x => x.RoundId);
            }
            else if (sortBy == RoundListingSortOrder.RoundListingSortEnum.RoundDateDescending)
            {
                queryable = queryable.OrderByDescending(x => x.RoundDate).ThenByDescending(x => x.RoundId);
            }
            else if (sortBy == RoundListingSortOrder.RoundListingSortEnum.BestScores)
            {
                queryable = queryable.OrderBy(x => x.Score).ThenBy(x => x.RoundId);
            }
            else if (sortBy == RoundListingSortOrder.RoundListingSortEnum.WorseScores)
            {
                queryable = queryable.OrderByDescending(x => x.Score).ThenByDescending(x => x.RoundId);
            }

            //go run the query now
            var dataSet = await queryable.Skip(skipAmount).Take(recordsPerPage).ToArrayAsync();

            //now grab the distinct course id's so we can get the images
            var distinctCourseIds = dataSet.Select(x => x.CourseId).Distinct();

            //now grab all the course images
            var courseImages = await dbContext.CourseImages.Where(x => distinctCourseIds.Contains(x.CourseId)).ToDictionaryAsync(x => x.CourseId, y => y.CourseImage);

            //let's loop through the rounds and display the starts
            foreach (var round in dataSet)
            {
                //calculate the round handicap
                round.RoundHandicap = Handicapper.RoundHandicap(round.Score, round.TeeBoxLocation.Rating, round.TeeBoxLocation.Slope);

                //calculate the adjusted score
                round.AdjustedScore = Convert.ToInt32(Math.Round(round.Score - round.HandicapBeforeRound, 0));

                //go calculate the round performance
                round.RoundPerformance = (int)RoundPerformance.CalculateRoundPerformance(round.TeeBoxLocation.Front9Par + round.TeeBoxLocation.Back9Par, round.AdjustedScore);
            }

            //go return the lookup now
            return new RoundSelectModel(courseImages, dataSet);
        }

        public static async Task<int> TotalNumberOfRounds(ToracGolfContext dbContext, int userId, string roundNameFilter, int? StateFilter, int RecordsPerPage)
        {
            return DataSetPaging.CalculateTotalPages(await RoundSelectQueryBuilder(dbContext, userId, roundNameFilter, StateFilter).CountAsync(), RecordsPerPage);
        }

        #endregion

    }
}

