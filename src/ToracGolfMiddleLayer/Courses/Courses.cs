using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToracGolf.MiddleLayer.EFModel;

namespace ToracGolf.MiddleLayer.Courses
{
    public static class Courses
    {

        public static async Task<int> CourseAdd(ToracGolfContext dbContext, int userId, CourseAddEnteredData CourseData)
        {
            //course record to add
            var courseToAdd = new EFModel.Tables.Course
            {
                Name = CourseData.CourseName,
                Description = CourseData.Description,
                IsActive = true,
                Pending = true,
                Location = CourseData.Location,
                StateId = Convert.ToInt32(CourseData.StateListing),
                UserIdThatCreatedCourse = userId
            };

            //go add the course record
            dbContext.Course.Add(courseToAdd);

            //go save it
            await dbContext.SaveChangesAsync();

            //return the course id
            return courseToAdd.CourseId;
        }

    }
}
