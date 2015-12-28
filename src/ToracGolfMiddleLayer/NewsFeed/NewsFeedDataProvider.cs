using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using ToracGolf.MiddleLayer.Courses;
using ToracGolf.MiddleLayer.EFModel;

namespace ToracGolf.MiddleLayer.NewsFeed
{

    public static class NewsFeedDataProvider
    {

        public static async Task<IEnumerable<NewsFeedItemModel>> NewsFeedPostSelect(ToracGolfContext dbContext, CourseImageFinder courseImageFinder, int userId)
        {
            var newsFeedItems = new List<NewsFeedItemModel>();

            //grab my rounds
            var myRounds = await dbContext.Rounds.AsNoTracking()
                                .Where(x => x.UserId == userId)
                                .OrderByDescending(x => x.RoundDate)
                                .Take(20)
                                .Select(y => new
                                {
                                    y.RoundDate,
                                    y.Score,
                                    y.CourseId,
                                    CourseName = y.Course.Name,
                                    TeeBoxDescription = y.CourseTeeLocation.Description
                                }).ToArrayAsync();

            newsFeedItems.AddRange(myRounds.Select(x => new NewRoundNewsFeed
            {
                CourseImagePath = courseImageFinder.FindCourseImage(x.CourseId),
                PostDate = x.RoundDate,
                CommentCount = 20,
                LikeCount = 30,
                TitleOfPost = string.Format($"You Scored A {x.Score} At {x.CourseName} - {x.TeeBoxDescription}")
            }));

            //go add the courses now
            var myCourses = await dbContext.Course.AsNoTracking()
                            .OrderBy(x => x.CreatedDate)
                            .Take(20)
                            .Select(x => new
                            {
                                CreatedDate = x.CreatedDate,
                                CourseName = x.Name,
                                CourseId = x.CourseId,
                                CourseDescription = x.Description,
                                StateTxt = x.State.Description,
                                City = x.City
                            }).ToArrayAsync();

            newsFeedItems.AddRange(myCourses.Select(x => new NewCourseNewsFeed
            {
                CourseImagePath = courseImageFinder.FindCourseImage(x.CourseId),
                CommentCount = 20,
                LikeCount = 20,
                PostDate = x.CreatedDate,
                TitleOfPost = string.Format($"{x.CourseName} In {x.City}, {x.StateTxt} Has Been Created."),
                BodyOfPost = x.CourseDescription
            }));

            return newsFeedItems.OrderByDescending(x => x.PostDate).ToArray();
        }

    }

}
