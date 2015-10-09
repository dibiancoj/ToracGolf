using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using ToracGolf.MiddleLayer.EFModel;
using ToracGolf.MiddleLayer.EFModel.Tables;
using ToracGolf.MiddleLayer.Rounds.Models;

namespace ToracGolf.MiddleLayer.Rounds
{
    public static class RoundDataProvider
    {

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

        public static async Task<IEnumerable<CourseTeeLocations>> TeeBoxSelectForCourse(ToracGolfContext dbContext, int courseId)
        {
            return await dbContext.CourseTeeLocations.AsNoTracking()
                         .Where(x => x.CourseId == courseId).ToArrayAsync();

        }

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
                SeasonId = seasonId
            };

            //add the round
            dbContext.Rounds.Add(round);

            //save the round
            await dbContext.SaveChangesAsync();

            //return the round id
            return round.RoundId;
        }

    }
}
