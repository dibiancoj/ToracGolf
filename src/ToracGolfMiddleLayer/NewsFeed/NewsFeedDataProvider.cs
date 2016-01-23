using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using ToracGolf.MiddleLayer.Courses;
using ToracGolf.MiddleLayer.EFModel;
using ToracGolf.MiddleLayer.EFModel.Tables;
using ToracGolf.MiddleLayer.NewsFeed.Models;
using ToracGolf.MiddleLayer.NewsFeed.Repository.Comments;
using ToracGolf.MiddleLayer.NewsFeed.Repository.Likes;

namespace ToracGolf.MiddleLayer.NewsFeed
{

    public static class NewsFeedDataProvider
    {

        public static async Task<NewsFeedQueryResult> NewsFeedPostSelect(ToracGolfContext dbContext,
                                                                                    CourseImageFinder courseImageFinder,
                                                                                    int userId,
                                                                                    NewsFeedItemModel.NewsFeedTypeId? newsFeedTypeIdFilter,
                                                                                    string searchFilterText)
        {
            var newsFeedItems = new List<NewsFeedItemModel>();

            int newRoundEnumValue = (int)NewsFeedItemModel.NewsFeedTypeId.NewRound;
            int newCourseEnumValue = (int)NewsFeedItemModel.NewsFeedTypeId.NewCourse;

            int newCourseCount;
            int newRoundCount;

            //grab my rounds

            var myRoundsQuery = dbContext.Rounds.AsNoTracking()
                                .Where(x => x.UserId == userId)
                                .OrderByDescending(x => x.RoundDate)
                                .Take(20).AsQueryable();

            if (!string.IsNullOrEmpty(searchFilterText))
            {
                myRoundsQuery = myRoundsQuery.Where(x => x.User.FirstName.Contains(searchFilterText) || x.User.LastName.Contains(searchFilterText) || x.Course.Name.Contains(searchFilterText));
            }

            newRoundCount = await myRoundsQuery.CountAsync();

            var myRounds = await myRoundsQuery.Select(y => new
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

            if (!newsFeedTypeIdFilter.HasValue || newsFeedTypeIdFilter.Value == NewsFeedItemModel.NewsFeedTypeId.NewRound)
            {
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


            //go add the courses now
            var myCoursesQueryable = dbContext.Course.AsNoTracking()
                        .OrderBy(x => x.CreatedDate)
                        .Take(20).AsQueryable();

            if (!string.IsNullOrEmpty(searchFilterText))
            {
                myCoursesQueryable = myCoursesQueryable.Where(x => x.Name.Contains(searchFilterText) || x.Description.Contains(searchFilterText) || x.City.Contains(searchFilterText));
            }

            newCourseCount = await myCoursesQueryable.CountAsync();

            var myCourses = await myCoursesQueryable.Select(x => new
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

            if (!newsFeedTypeIdFilter.HasValue || newsFeedTypeIdFilter.Value == NewsFeedItemModel.NewsFeedTypeId.NewCourse)
            {
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

            return new NewsFeedQueryResult(newsFeedItems.OrderByDescending(x => x.PostDate).ToArray(), newRoundCount, newCourseCount);
        }

        public static async Task<bool> NewsFeedLikeAdd(ToracGolfContext dbContext, int userId, int id, NewsFeedItemModel.NewsFeedTypeId newsFeedTypeId)
        {
            INewsFeedLikeRepository repository = new NewsFeedLikeRepository(dbContext);

            //so you like the item already?
            var alreadyLikeItem = await repository.GetLikes().FirstOrDefaultAsync(x => x.AreaId == id && x.NewsFeedTypeId == (int)newsFeedTypeId && x.UserIdThatLikedItem == userId);

            //did we find this record? if so we delete it
            if (alreadyLikeItem != null)
            {
                //we want to remove this record
                return await repository.Delete(alreadyLikeItem);
            }

            //else we don't have a record, we are in add mode
            return await repository.Add(new NewsFeedLike
            {
                AreaId = id,
                NewsFeedTypeId = (int)newsFeedTypeId,
                UserIdThatLikedItem = userId,
                CreatedDate = DateTime.Now
            });
        }

        #region Comments

        public static async Task<IEnumerable<NewsFeedCommentRow>> CommentAdd(ToracGolfContext dbContext, int userId, int id, NewsFeedItemModel.NewsFeedTypeId newsFeedTypeId, string commentToAdd)
        {
            INewsFeedCommentRepository repository = new NewsFeedCommentRepository(dbContext);

            //lets go add the row
            await repository.Add(new NewsFeedComment
            {
                AreaId = id,
                NewsFeedTypeId = (int)newsFeedTypeId,
                UserIdThatCommented = userId,
                CreatedDate = DateTime.Now,
                Comment = commentToAdd
            });

            //go return this posts comment
            return await CommentSelect(dbContext, userId, id, newsFeedTypeId);
        }

        public static async Task<IEnumerable<NewsFeedCommentRow>> CommentSelect(ToracGolfContext dbContext, int userId, int id, NewsFeedItemModel.NewsFeedTypeId newsFeedTypeId)
        {
            INewsFeedCommentRepository repository = new NewsFeedCommentRepository(dbContext);

            return await repository.GetComments().AsNoTracking()
                         .Where(x => x.AreaId == id && x.NewsFeedTypeId == (int)newsFeedTypeId)
                         .OrderBy(x => x.CreatedDate)
                         .Select(repository.SelectCommand).ToArrayAsync();

        }

        public static async Task<int> CommentLikeAddOrRemove(ToracGolfContext dbContext, int userId, int commentId)
        {
            //let's try to grab this by id
            INewsFeedLikeRepository likeRepository = new NewsFeedLikeRepository(dbContext);

            //does this user already like this comment? (we don't have a like repository yet).
            var userLikesCommentAlready = await likeRepository.GetLikesByIdAndUserId(commentId, NewsFeedItemModel.NewsFeedTypeId.Comment, userId);

            //do we already have this item
            if (userLikesCommentAlready == null)
            {
                //add the comment
                await likeRepository.Add(new NewsFeedLike
                {
                    AreaId = commentId,
                    CreatedDate = DateTime.Now,
                    NewsFeedTypeId = (int)NewsFeedItemModel.NewsFeedTypeId.Comment,
                    UserIdThatLikedItem = userId
                });
            }
            else
            {
                await likeRepository.Delete(userLikesCommentAlready);
            }

            //return the new count
            return await likeRepository.GetLikes().AsNoTracking().CountAsync(x => x.AreaId == commentId && x.NewsFeedTypeId == (int)NewsFeedItemModel.NewsFeedTypeId.Comment);
        }

        #endregion

    }

}
