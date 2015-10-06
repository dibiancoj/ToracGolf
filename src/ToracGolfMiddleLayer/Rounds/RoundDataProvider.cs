using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using ToracGolf.MiddleLayer.EFModel;
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
                           TeeLocations = dbContext.CourseTeeLocations.Where(y => y.CourseId == x.CourseId).OrderBy(y => y.TeeLocationSortOrderId).Select(y => new TeeBoxForRoundAddScreen
                           {
                               CourseTeeLocationId = y.CourseTeeLocationId,
                               Description = y.Description,
                               Front9Par = y.Front9Par,
                               Back9Par = y.Back9Par,
                               Rating = y.Rating,
                               Slope = y.Slope,
                               Yardage = y.Yardage
                           })
                       }).ToArrayAsync();
        }

    }
}
