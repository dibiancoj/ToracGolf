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

        public static async Task<IEnumerable<NewsFeedItemModel>> NewsFeedPostSelect(ToracGolfContext dbContext, CourseImageFinder courseImageFinder, int userId, NewsFeedItemModel.NewsFeedTypeId? newsFeedTypeIdFilter)
        {
            var newsFeedItems = new List<NewsFeedItemModel>();

            int newRoundEnumValue = (int)NewsFeedItemModel.NewsFeedTypeId.NewRound;
            int newCourseEnumValue = (int)NewsFeedItemModel.NewsFeedTypeId.NewCourse;

            //grab my rounds
            if (!newsFeedTypeIdFilter.HasValue || newsFeedTypeIdFilter.Value == NewsFeedItemModel.NewsFeedTypeId.NewRound)
            {
                var myRounds = await dbContext.Rounds.AsNoTracking()
                                    .Where(x => x.UserId == userId)
                                    .OrderByDescending(x => x.RoundDate)
                                    .Take(20)
                                    .Select(y => new
                                    {
                                        y.RoundId,
                                        y.RoundDate,
                                        y.Score,
                                        y.CourseId,
                                        CourseName = y.Course.Name,
                                        TeeBoxDescription = y.CourseTeeLocation.Description,
                                        Likes = dbContext.NewsFeedLike.Count(x => x.NewsFeedTypeId == newRoundEnumValue && x.AreaId == y.RoundId),
                                        Comments = dbContext.NewsFeedComment.Count(x => x.NewsFeedTypeId == newRoundEnumValue && x.AreaId == y.RoundId),
                                        AdjustedScore = y.Score - y.Handicap.HandicapBeforeRound,
                                        RoundHandicap = y.RoundHandicap,
                                        YouLikedItem = dbContext.NewsFeedLike.Any(x => x.AreaId == y.RoundId && x.NewsFeedTypeId == newRoundEnumValue && x.UserIdThatLikedItem == userId)
                                    }).ToArrayAsync();

                newsFeedItems.AddRange(myRounds.Select(x => new NewRoundNewsFeed
                {
                    Id = x.RoundId,
                    CourseImagePath = courseImageFinder.FindCourseImage(x.CourseId),
                    PostDate = x.RoundDate,
                    CommentCount = x.Comments,
                    LikeCount = x.Likes,
                    YouLikedItem = x.YouLikedItem,
                    TitleOfPost = string.Format($"You Scored A {x.Score} At {x.CourseName} - {x.TeeBoxDescription}"),
                    BodyOfPost = new string[]
                    {
                    string.Format($"Adjusted Score: {Convert.ToInt32(Math.Round(x.AdjustedScore, 1))}"),
                    string.Format($"Round Handicap: {Math.Round(x.RoundHandicap, 2)}")
                    }
                }));
            }

            if (!newsFeedTypeIdFilter.HasValue || newsFeedTypeIdFilter.Value == NewsFeedItemModel.NewsFeedTypeId.NewCourse)
            {
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
                                City = x.City,
                                Likes = dbContext.NewsFeedLike.Count(y => y.NewsFeedTypeId == newCourseEnumValue && y.AreaId == x.CourseId),
                                Comments = dbContext.NewsFeedComment.Count(y => y.NewsFeedTypeId == newCourseEnumValue && y.AreaId == x.CourseId),
                                YouLikedItem = dbContext.NewsFeedLike.Any(y => x.CourseId == y.AreaId && y.NewsFeedTypeId == newCourseEnumValue && y.UserIdThatLikedItem == userId)
                            }).ToArrayAsync();

                newsFeedItems.AddRange(myCourses.Select(x => new NewCourseNewsFeed
                {
                    Id = x.CourseId,
                    CourseImagePath = courseImageFinder.FindCourseImage(x.CourseId),
                    CommentCount = x.Comments,
                    LikeCount = x.Likes,
                    PostDate = x.CreatedDate,
                    YouLikedItem = x.YouLikedItem,
                    TitleOfPost = string.Format($"{x.CourseName} In {x.City}, {x.StateTxt} Has Been Created."),
                    BodyOfPost = new string[] { x.CourseDescription }
                }));
            }

            return newsFeedItems.OrderByDescending(x => x.PostDate).ToArray();
        }

        public static async Task<bool> NewsFeedLikeAdd(ToracGolfContext dbContext, int userId, int id, NewsFeedItemModel.NewsFeedTypeId newsFeedTypeId)
        {
            //so you like the item already?
            var alreadyLikeItem = await dbContext.NewsFeedLike.FirstOrDefaultAsync(x => x.AreaId == id && x.NewsFeedTypeId == (int)newsFeedTypeId && x.UserIdThatLikedItem == userId);

            if (alreadyLikeItem != null)
            {
                dbContext.NewsFeedLike.Remove(alreadyLikeItem);
            }
            else {
                dbContext.NewsFeedLike.Add(new EFModel.Tables.NewsFeedLike
                {
                    AreaId = id,
                    NewsFeedTypeId = (int)newsFeedTypeId,
                    UserIdThatLikedItem = userId,
                    CreatedDate = DateTime.Now
                });
            }

            await dbContext.SaveChangesAsync();

            return true;
        }

    }

}
