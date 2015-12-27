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
                                .Take(50)
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

            return newsFeedItems.OrderByDescending(x => x.PostDate).ToArray();
        }

    }

}
