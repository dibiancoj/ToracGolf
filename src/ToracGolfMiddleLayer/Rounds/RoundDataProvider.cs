using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using ToracGolf.MiddleLayer.Courses;
using ToracGolf.MiddleLayer.EFModel;
using ToracGolf.MiddleLayer.EFModel.Tables;
using ToracGolf.MiddleLayer.GridCommon;
using ToracGolf.MiddleLayer.HandicapCalculator;
using ToracGolf.MiddleLayer.HandicapCalculator.Models;
using ToracGolf.MiddleLayer.ListingFactories;
using ToracGolf.MiddleLayer.Rounds.Models;
using ToracLibrary.AspNet.Paging;

namespace ToracGolf.MiddleLayer.Rounds
{
    public static class RoundDataProvider
    {

        #region General Lookups

        public static async Task<IEnumerable<CourseForRoundAddScreen>> ActiveCoursesSelectForState(ToracGolfContext dbContext, int stateId)
        {
            return await dbContext.Course.AsNoTracking()
                         .Where(x => x.StateId == stateId && x.IsActive)
                         .OrderBy(x => x.Name)
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
                teeBoxModel.CourseTeeBoxHandicap = Handicapper.CalculateCourseHandicap(currentHandicap, teeBoxModel.Slope);

                //now calculate the score
                teeBoxModel.MaxScorePerHole = Handicapper.MaxScorePerHole(teeBoxModel.CourseTeeBoxHandicap);

                //add the tee box score
                lst.Add(teeBoxModel);
            }

            //return the list
            return lst;
        }

        #endregion

        #region Round Add

        public static async Task<int> SaveRound(ToracGolfContext dbContext, int userId, int seasonId, RoundAddEnteredData roundData)
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
                SeasonId = seasonId,
                GreensInRegulation = roundData.GreensInRegulation,
                Putts = roundData.Putts,
                FairwaysHit = roundData.FairwaysHit
            };

            //go grab the tee location 
            var teeLocation = await dbContext.CourseTeeLocations.AsNoTracking().FirstAsync(x => x.CourseTeeLocationId == roundData.TeeLocationId);

            //add the round handicap now
            round.RoundHandicap = Handicapper.CalculateHandicap(new Last20Rounds[] { new Last20Rounds { Rating = teeLocation.Rating, Slope = teeLocation.Slope, RoundScore = round.Score } }).Value;

            //add the round
            dbContext.Rounds.Add(round);

            //save the round
            //await dbContext.SaveChangesAsync();
            dbContext.SaveChanges();

            //we need to go add the handicap records
            await RefreshHandicapRecords(dbContext, userId);

            //return the round id
            return round.RoundId;
        }

        #endregion

        #region Handicap Refresh

        public static async Task<bool> RefreshHandicapRecords(ToracGolfContext dbContext, int userId)
        {
            //let's go delete all the handicap records
            var roundsToDelete = await (from myData in dbContext.Rounds
                                        join myHandicaps in dbContext.Handicap
                                        on myData.RoundId equals myHandicaps.RoundId
                                        where myData.UserId == userId
                                        select myHandicaps).ToArrayAsync();

            //delete all handicap records
            dbContext.Handicap.RemoveRange(roundsToDelete);

            //save the deletions
            await dbContext.SaveChangesAsync();

            //5. let's go rebuild all the handicap records
            var roundsForUser = await dbContext.Rounds.Include(x => x.CourseTeeLocation).Where(x => x.UserId == userId)
                                .OrderBy(x => x.RoundDate).ThenBy(x => x.RoundId)
                                .Select(x => x).ToArrayAsync();

            //func to go from rounds to last 20 rounds
            Func<Round, Last20Rounds> roundToLast20 = x => new Last20Rounds
            {
                RoundId = x.RoundId,
                RoundScore = x.Score,
                Rating = x.CourseTeeLocation.Rating,
                Slope = x.CourseTeeLocation.Slope
            };

            //lets loop through those rounds for the user
            foreach (var roundToCalculate in roundsForUser)
            {
                //grab the 20 rounds before this round
                var roundsToUse = roundsForUser
                    .Where(x => x.RoundDate < roundToCalculate.RoundDate)
                    .OrderByDescending(x => x.RoundDate)
                    .ThenByDescending(x => x.RoundId)
                    .Take(20)
                    .Select(roundToLast20).ToArray();

                //current round we are up too
                var currentRoundInArrayFormat = new Last20Rounds[] { roundToLast20(roundToCalculate) };

                //rounds for the "After round handicap"
                ICollection<Last20Rounds> roundsToCalculateAfterRoundHandicap;

                //if we have 0 rounds, then use the round we are looking through
                if (!roundsToUse.Any())
                {
                    //set the current rounds to just this one guy
                    roundsToUse = currentRoundInArrayFormat;

                    //set it to just this round
                    roundsToCalculateAfterRoundHandicap = roundsToUse;
                }
                else
                {
                    roundsToCalculateAfterRoundHandicap = roundsToUse.Concat(currentRoundInArrayFormat).ToArray();
                }

                //add the record to the context
                dbContext.Handicap.Add(new Handicap
                {
                    RoundId = roundToCalculate.RoundId,
                    HandicapBeforeRound = Handicapper.CalculateHandicap(roundsToUse).Value,
                    HandicapAfterRound = Handicapper.CalculateHandicap(roundsToCalculateAfterRoundHandicap).Value
                });
            }

            //go save the changes for the course handicap
            await dbContext.SaveChangesAsync();

            //return a postive result
            return true;
        }

        #endregion

        #region Round Listing

        public static IQueryable<RoundListingData> RoundSelectQueryBuilder(ToracGolfContext dbContext, int userId, string courseNameFilter, int? seasonFilter, DateTime? roundDateStartFilter, DateTime? roundDateEndFilter, bool handicappedRoundsOnly)
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

            //do we have start date filter?
            if (roundDateStartFilter.HasValue)
            {
                queryable = queryable.Where(x => x.RoundDate >= roundDateStartFilter.Value);
            }

            //end date filter?
            if (roundDateEndFilter.HasValue)
            {
                queryable = queryable.Where(x => x.RoundDate <= roundDateEndFilter.Value);
            }

            if (handicappedRoundsOnly)
            {
                //how many rounds do we have?
                int roundsWeHaveInQuery = queryable.Count();

                //now check how many are in the calculation
                var howManyToCalculateWith = Handicapper.HowManyRoundsToUseInFormula(roundsWeHaveInQuery > 20 ? 20 : roundsWeHaveInQuery);

                //now just grab the last 20
                var last20RoundIds = queryable.OrderByDescending(x => x.RoundDate)
                                              .ThenByDescending(x => x.RoundId)
                                              .Take(20) //we only ever get the last 20...
                                              .OrderBy(x => x.RoundHandicap) //now grab the lowest rated rounds of how many we are going to calculate with
                                              .Take(howManyToCalculateWith) //then grab just those
                                              .Select(x => x.RoundId).ToArray();

                //add the round id's to the query
                queryable = queryable.Where(x => last20RoundIds.Contains(x.RoundId));
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
                HandicapBeforeRound = dbContext.Handicap.FirstOrDefault(y => y.RoundId == x.RoundId).HandicapBeforeRound,

                Putts = x.Putts,
                FairwaysHit = x.FairwaysHit,
                GreensInRegulation = x.GreensInRegulation,

                RoundHandicap = x.RoundHandicap
            });
        }

        public static async Task<RoundSelectModel> RoundSelect(IListingFactory<RoundListingData> roundListingFactory,
                                                               ToracGolfContext dbContext,
                                                               int userId,
                                                               int pageId,
                                                               RoundListingSortOrder.RoundListingSortEnum sortBy,
                                                               string courseNameFilter,
                                                               int? seasonFilter,
                                                               int recordsPerPage,
                                                               DateTime? roundDateStartFilter,
                                                               DateTime? roundDateEndFilter,
                                                               CourseImageFinder courseImageFinder,
                                                               bool handicappedRoundOnly)
        {
            //how many items to skip
            int skipAmount = pageId * recordsPerPage;

            //go grab the query
            var queryable = RoundSelectQueryBuilder(dbContext, userId, courseNameFilter, seasonFilter, roundDateStartFilter, roundDateEndFilter, handicappedRoundOnly);

            //go sort the data
            var sortedQueryable = roundListingFactory.SortByConfiguration[sortBy.ToString()](queryable, new ListingFactoryParameters(dbContext, userId)).ThenBy(x => x.RoundId);

            //go run the query now
            var dataSet = await EFPaging.PageEfQuery(sortedQueryable, pageId, recordsPerPage).ToListAsync();

            //let's loop through the rounds and display the starts
            foreach (var round in dataSet)
            {
                //calculate the adjusted score
                round.AdjustedScore = Convert.ToInt32(Math.Round(round.Score - round.HandicapBeforeRound, 0));

                //go calculate the round performance
                round.RoundPerformance = (int)RoundPerformance.CalculateRoundPerformance(round.TeeBoxLocation.Front9Par + round.TeeBoxLocation.Back9Par, round.AdjustedScore);

                //set the image path
                round.CourseImagePath = courseImageFinder.FindCourseImage(round.CourseId);
            }

            //go return the lookup now
            return new RoundSelectModel(dataSet);
        }

        public static async Task<int> TotalNumberOfRounds(ToracGolfContext dbContext, int userId, string roundNameFilter, int? StateFilter, DateTime? roundDateStartFilter, DateTime? roundDateEndFilter, bool onlyHandicappedRounds)
        {
            return await RoundSelectQueryBuilder(dbContext, userId, roundNameFilter, StateFilter, roundDateStartFilter, roundDateEndFilter, onlyHandicappedRounds).CountAsync();
        }

        #endregion

        #region Delete A Round

        public static async Task<bool> DeleteARound(ToracGolfContext dbContext, int roundIdToDelete)
        {
            //grab the round
            var roundToDelete = await dbContext.Rounds.FirstOrDefaultAsync(x => x.RoundId == roundIdToDelete);

            //remove the round
            dbContext.Rounds.Remove(roundToDelete);

            //save the changes
            await dbContext.SaveChangesAsync();

            //return a true result
            return true;
        }

        #endregion

    }
}

