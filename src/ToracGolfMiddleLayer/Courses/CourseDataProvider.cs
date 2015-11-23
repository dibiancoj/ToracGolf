using Microsoft.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToracGolf.MiddleLayer.EFModel;
using ToracGolf.MiddleLayer.EFModel.Tables;
using ToracLibrary.AspNet.Paging;

namespace ToracGolf.MiddleLayer.Courses
{
    public static class CourseDataProvider
    {

        #region Course Add

        public static async Task<int> CourseAdd(ToracGolfContext dbContext, int userId, CourseAddEnteredData CourseData)
        {
            //course record to add
            var courseToAdd = new Course
            {
                Name = CourseData.CourseName,
                Description = CourseData.Description,
                IsActive = true,
                Pending = true,
                City = CourseData.City,
                StateId = Convert.ToInt32(CourseData.StateListing),
                UserIdThatCreatedCourse = userId,
                OnlyAllow18Holes = CourseData.OnlyAllow18Holes
            };

            courseToAdd.CourseTeeLocations = CourseData.TeeLocations.Select((x, i) => new CourseTeeLocations
            {
                CourseId = courseToAdd.CourseId,
                Description = x.Description,
                TeeLocationSortOrderId = i,
                Front9Par = x.Front9Par.Value,
                Back9Par = x.Back9Par.Value,
                Rating = x.Rating.Value,
                Slope = x.Slope.Value,
                Yardage = x.Yardage.Value
            }).ToArray();

            //add the course to the context
            dbContext.Course.Add(courseToAdd);

            //go save it
            await dbContext.SaveChangesAsync();

            //do we have a course image?
            if (CourseData.CourseImage != null)
            {
                //grab the byte array for the file
                byte[] fileToSave = ToracLibrary.AspNet.Graphics.GraphicsUtilities.ImageFromJsonBase64String(CourseData.CourseImage).FileBytes;

                //let's go save the image
                dbContext.CourseImages.Add(new CourseImages { CourseId = courseToAdd.CourseId, CourseImage = fileToSave });

                //save it now
                await dbContext.SaveChangesAsync();
            }

            //return the course id
            return courseToAdd.CourseId;
        }

        #endregion

        #region Course Listing

        public static IQueryable<Course> CourseSelectQueryBuilder(ToracGolfContext dbContext, string courseNameFilter, int? StateFilter)
        {
            //build the queryable
            var queryable = dbContext.Course.AsNoTracking().Where(x => x.IsActive).AsQueryable();

            //if we have a course name, add it as a filter
            if (!string.IsNullOrEmpty(courseNameFilter))
            {
                queryable = queryable.Where(x => x.Name.Contains(courseNameFilter));
            }

            //do we have a state filter?
            if (StateFilter.HasValue)
            {
                queryable = queryable.Where(x => x.StateId == StateFilter.Value);
            }

            //return the queryable
            return queryable;
        }

        /// <param name="pageId">0 base index that holds what page we are on</param>
        public static async Task<IEnumerable<CourseListingData>> CourseSelect(ToracGolfContext dbContext,
                                                                              int pageId,
                                                                              CourseListingSortOrder.CourseListingSortEnum sortBy,
                                                                              string courseNameFilter,
                                                                              int? stateFilter,
                                                                              int recordsPerPage,
                                                                              int userId)
        {
            //how many items to skip
            int skipAmount = pageId * recordsPerPage;

            //go grab the query
            var queryable = CourseSelectQueryBuilder(dbContext, courseNameFilter, stateFilter);

            //figure out what you want to order by
            if (sortBy == CourseListingSortOrder.CourseListingSortEnum.CourseNameAscending)
            {
                queryable = queryable.OrderBy(x => x.Name);
            }
            else if (sortBy == CourseListingSortOrder.CourseListingSortEnum.CourseNameDescending)
            {
                queryable = queryable.OrderByDescending(x => x.Name);
            }
            else if (sortBy == CourseListingSortOrder.CourseListingSortEnum.EasiestCourses)
            {
                queryable = queryable.OrderBy(x => x.CourseTeeLocations.Min(y => y.Slope));
            }
            else if (sortBy == CourseListingSortOrder.CourseListingSortEnum.HardestCourses)
            {
                queryable = queryable.OrderByDescending(x => x.CourseTeeLocations.Max(y => y.Slope));
            }
            else if (sortBy == CourseListingSortOrder.CourseListingSortEnum.MostTimesPlayed)
            {
                //todo: need to fix when we get the rounds table going
                queryable = queryable.OrderBy(x => x.Name);
            }

            //go run the query now
            return await queryable.Select(x => new CourseListingData
            {
                CourseData = x,
                StateDescription = dbContext.Ref_State.FirstOrDefault(y => y.StateId == x.StateId).Description,
                TeeLocationCount = x.CourseTeeLocations.Count,
                CourseImage = dbContext.CourseImages.FirstOrDefault(y => y.CourseId == x.CourseId),

                NumberOfRounds = dbContext.Rounds.Count(y => y.CourseId == x.CourseId && y.UserId == userId),
                TopScore = dbContext.Rounds.Where(y => y.CourseId == x.CourseId && y.UserId == userId).Select(y => y.Score).Min(),
                WorseScore = dbContext.Rounds.Where(y => y.CourseId == x.CourseId && y.UserId == userId).Select(y => y.Score).Max(),
                AverageScore = dbContext.Rounds.Where(y => y.CourseId == x.CourseId && y.UserId == userId).Select(y => y.Score).Average()
            }).Skip(skipAmount).Take(recordsPerPage).ToArrayAsync();
        }

        public static async Task<int> TotalNumberOfCourses(ToracGolfContext dbContext, string courseNameFilter, int? StateFilter, int RecordsPerPage)
        {
            return DataSetPaging.CalculateTotalPages(await CourseSelectQueryBuilder(dbContext, courseNameFilter, StateFilter).CountAsync(), RecordsPerPage);
        }

        public static async Task<Tuple<string, string>> CourseNameAndState(ToracGolfContext dbContext, int courseId)
        {
            //grab the course
            var course = await dbContext.Course.AsNoTracking().Where(x => x.CourseId == courseId).Select(x => new { x.Name, x.StateId }).FirstAsync();

            //return the tuple
            return new Tuple<string, string>(course.Name, course.StateId.ToString());
        }

        public static async Task<bool> DeleteACourse(ToracGolfContext dbContext, int courseId)
        {
            //grab the course
            var courseToDelete = await dbContext.Course.FirstAsync(x => x.CourseId == courseId);

            //now flip the flag on the course
            courseToDelete.IsActive = false;

            //save the changes
            await dbContext.SaveChangesAsync();

            //return a positive result
            return true;
        }

        #endregion

    }
}
