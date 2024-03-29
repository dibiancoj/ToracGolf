﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using ToracGolf.MiddleLayer.Courses;
using ToracGolf.MiddleLayer.EFModel;
using ToracGolf.MiddleLayer.EFModel.Tables;
using ToracGolf.MiddleLayer.Friend;
using ToracGolf.MiddleLayer.NewsFeed.Models;

namespace ToracGolf.MiddleLayer.NewsFeed.Repository.GridQueries
{

    public class NewsFeedGridQueries : INewsFeedGridQueries
    {

        #region Constructor

        public NewsFeedGridQueries(ToracGolfContext dbContext)
        {
            DbContext = dbContext;
        }

        #endregion

        #region Immutable Properties

        private ToracGolfContext DbContext { get; }

        #endregion

        #region Methods

        public async Task<NewsFeedQueryResult> NewsFeedPostSelect(int userId, NewsFeedItemModel.NewsFeedTypeId? newsFeedTypeIdFilter, string searchFilterText, ImageFinder courseImageFinder, ImageFinder userImageLocator)
        {
            const int takeAmount = 20;

            var newsFeedItems = new List<NewsFeedItemModel>();

            int newCourseCount;
            int newRoundCount;

            //grab the user's friends
            var usersFriendIds = await FriendsDataProvider.GetUsersFriendList(DbContext, userId).ToArrayAsync();

            //let's go grab the round query
            var roundQuery = RoundGet().AsNoTracking()
                            .Where(x => x.UserId == userId || usersFriendIds.Contains(x.UserId))
                            .OrderByDescending(x => x.RoundDate)
                            .Take(takeAmount);

            //go build the course query
            var courseQuery = CourseGet().AsNoTracking()
                              .OrderByDescending(x => x.CreatedDate)
                              .Take(takeAmount);

            //add the filters now
            if (!string.IsNullOrEmpty(searchFilterText))
            {
                //round query filter
                roundQuery = roundQuery.Where(x => x.User.FirstName.Contains(searchFilterText) || x.User.LastName.Contains(searchFilterText) || x.Course.Name.Contains(searchFilterText));

                //course query filter
                courseQuery = courseQuery.Where(x => x.Name.Contains(searchFilterText) || x.Description.Contains(searchFilterText) || x.City.Contains(searchFilterText));
            }

            //grab the count of records we have before we go filter by news feed filter type, we always want the count of all items
            newRoundCount = await roundQuery.CountAsync();
            newCourseCount = await courseQuery.CountAsync();

            //do they want to filter one or the other?
            //only rounds?
            if (ShouldRunQuery(NewsFeedItemModel.NewsFeedTypeId.NewRound, newsFeedTypeIdFilter))
            {
                newsFeedItems.AddRange(await RoundSelect(roundQuery, userId, courseImageFinder, userImageLocator));
            }

            if (ShouldRunQuery(NewsFeedItemModel.NewsFeedTypeId.NewCourse, newsFeedTypeIdFilter))
            {
                newsFeedItems.AddRange(await CourseSelect(courseQuery, userId, courseImageFinder));
            }

            //now return the model
            return new NewsFeedQueryResult(newsFeedItems.OrderByDescending(x => x.PostDate).ToArray(), newRoundCount, newCourseCount, usersFriendIds.Length);
        }

        #endregion

        #region Private Helper Methods

        private static bool ShouldRunQuery(NewsFeedItemModel.NewsFeedTypeId itemToTest, NewsFeedItemModel.NewsFeedTypeId? filterValue)
        {
            return !filterValue.HasValue || filterValue.Value == itemToTest;
        }

        #endregion

        #region Private Repository Methods

        #region Round Repository

        private IQueryable<Round> RoundGet()
        {
            return DbContext.Rounds.AsQueryable();
        }

        private async Task<IEnumerable<NewRoundNewsFeed>> RoundSelect(IQueryable<Round> query, int userId, ImageFinder courseImageFinder, ImageFinder userImageFinder)
        {
            int newRoundTypeId = (int)NewsFeedItemModel.NewsFeedTypeId.NewRound;

            return (await query.Select(y => new
            {
                UserId = y.UserId,
                UserName = y.User.FirstName + " " + y.User.LastName,
                RoundId = y.RoundId,
                RoundDate = y.RoundDate,
                Score = y.Score,
                CourseId = y.CourseId,
                CourseName = y.Course.Name,
                TeeBoxDescription = y.CourseTeeLocation.Description,
                Likes = DbContext.NewsFeedLike.Count(x => x.NewsFeedTypeId == newRoundTypeId && x.AreaId == y.RoundId),
                Comments = DbContext.NewsFeedComment.Count(x => x.NewsFeedTypeId == newRoundTypeId && x.AreaId == y.RoundId),
                AdjustedScore = y.Score - y.Handicap.HandicapBeforeRound,
                RoundHandicap = y.RoundHandicap,
                YouLikedItem = DbContext.NewsFeedLike.Any(x => x.AreaId == y.RoundId && x.NewsFeedTypeId == newRoundTypeId && x.UserIdThatLikedItem == userId)
            }).ToArrayAsync()).Select(x => new NewRoundNewsFeed
            {
                Id = x.RoundId,
                CourseId = x.CourseId,
                CourseImagePath = courseImageFinder.FindImage(x.CourseId),
                PostDate = x.RoundDate,
                CommentCount = x.Comments,
                LikeCount = x.Likes,
                YouLikedItem = x.YouLikedItem,
                TitleOfPost = string.Format($"{(x.UserId == userId ? "You" : x.UserName)} Scored A {x.Score} At {x.CourseName} - {x.TeeBoxDescription}"),
                BodyOfPost = new NewRoundBody
                {
                    AdjustedScore = Convert.ToInt32(Math.Round(x.AdjustedScore, 1)),
                    RoundHandicap = Math.Round(x.RoundHandicap, 2),
                    UserImageUrl = userImageFinder.FindImage(x.UserId)
                }
            });
        }

        #endregion

        #region Course Repository

        private IQueryable<Course> CourseGet()
        {
            return DbContext.Course.AsQueryable();
        }

        private async Task<IEnumerable<NewCourseNewsFeed>> CourseSelect(IQueryable<Course> query, int userId, ImageFinder courseImageFinder)
        {
            int newCourseTypeId = (int)NewsFeedItemModel.NewsFeedTypeId.NewCourse;

            return (await query.Select(x => new
            {
                CreatedDate = x.CreatedDate,
                CourseName = x.Name,
                CourseId = x.CourseId,
                CourseDescription = x.Description,
                StateTxt = x.State.Description,
                City = x.City,
                Likes = DbContext.NewsFeedLike.Count(y => y.NewsFeedTypeId == newCourseTypeId && y.AreaId == x.CourseId),
                Comments = DbContext.NewsFeedComment.Count(y => y.NewsFeedTypeId == newCourseTypeId && y.AreaId == x.CourseId),
                YouLikedItem = DbContext.NewsFeedLike.Any(y => x.CourseId == y.AreaId && y.NewsFeedTypeId == newCourseTypeId && y.UserIdThatLikedItem == userId)
            }).ToArrayAsync()).Select(x => new NewCourseNewsFeed
            {
                Id = x.CourseId,
                CourseId = x.CourseId,
                CourseImagePath = courseImageFinder.FindImage(x.CourseId),
                CommentCount = x.Comments,
                LikeCount = x.Likes,
                PostDate = x.CreatedDate,
                YouLikedItem = x.YouLikedItem,
                TitleOfPost = string.Format($"{x.CourseName} In {x.City}, {x.StateTxt} Has Been Created."),
                BodyOfPost = new NewCourseBody { NewCourseStory = x.CourseDescription }
            });
        }

        #endregion

        #endregion

    }

}
