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
using ToracGolf.MiddleLayer.NewsFeed.Repository.GridQueries;
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
            return await new NewsFeedGridQueries(dbContext, courseImageFinder).NewsFeedPostSelect(userId, newsFeedTypeIdFilter, searchFilterText);
        }

        #region Likes

        public static async Task<bool> NewsFeedLikeAdd(ToracGolfContext dbContext, int userId, int id, NewsFeedItemModel.NewsFeedTypeId newsFeedTypeId)
        {
            INewsFeedLikeRepository repository = new NewsFeedLikeRepository(dbContext);

            return await repository.AddOrRemoveLike(id, newsFeedTypeId, userId);
        }

        #endregion

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
            //create the repository
            INewsFeedLikeRepository likeRepository = new NewsFeedLikeRepository(dbContext);

            //go take care of the like
            await likeRepository.AddOrRemoveLike(commentId, NewsFeedItemModel.NewsFeedTypeId.Comment, userId);

            //return the new count
            return await likeRepository.GetLikes().AsNoTracking().CountAsync(x => x.AreaId == commentId && x.NewsFeedTypeId == (int)NewsFeedItemModel.NewsFeedTypeId.Comment);
        }

        #endregion

    }

}
