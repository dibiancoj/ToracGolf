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

    public class NewsFeedDataProvider
    {

        #region Constructor

        public NewsFeedDataProvider(INewsFeedGridQueries gridQueryRepository, INewsFeedLikeRepository likeRepository, INewsFeedCommentRepository commentRepository)
        {
            GridQueryRepository = gridQueryRepository;
            LikeRepository = likeRepository;
            CommentRepository = commentRepository;
        }

        #endregion

        #region Properties

        public INewsFeedGridQueries GridQueryRepository { get; }
        public INewsFeedCommentRepository CommentRepository { get; }
        public INewsFeedLikeRepository LikeRepository { get; }

        #endregion

        #region Query 

        public async Task<NewsFeedQueryResult> NewsFeedPostSelect(int userId, NewsFeedItemModel.NewsFeedTypeId? newsFeedTypeIdFilter, string searchFilterText, ImageFinder courseImageLocator, ImageFinder userImageLocator)
        {
            return await GridQueryRepository.NewsFeedPostSelect(userId, newsFeedTypeIdFilter, searchFilterText, courseImageLocator, userImageLocator).ConfigureAwait(false);
        }

        #endregion

        #region Likes

        public async Task<bool> NewsFeedLikeAdd(ToracGolfContext dbContext, int userId, int id, NewsFeedItemModel.NewsFeedTypeId newsFeedTypeId)
        {
            INewsFeedLikeRepository repository = new NewsFeedLikeRepository(dbContext);

            return await repository.AddOrRemoveLike(id, newsFeedTypeId, userId);
        }

        #endregion

        #region Comments

        public async Task<IEnumerable<NewsFeedCommentRow>> CommentAdd(ToracGolfContext dbContext, int userId, int id, NewsFeedItemModel.NewsFeedTypeId newsFeedTypeId, string commentToAdd, ImageFinder userImageFinder)
        {
            //lets go add the row
            await CommentRepository.Add(new NewsFeedComment
            {
                AreaId = id,
                NewsFeedTypeId = (int)newsFeedTypeId,
                UserIdThatCommented = userId,
                CreatedDate = DateTime.Now,
                Comment = commentToAdd
            });

            //go return this posts comment
            return await CommentSelect(dbContext, userId, id, newsFeedTypeId, userImageFinder);
        }

        public async Task<IEnumerable<NewsFeedCommentRow>> CommentSelect(ToracGolfContext dbContext, int userId, int id, NewsFeedItemModel.NewsFeedTypeId newsFeedTypeId, ImageFinder userImageFinder)
        {
            var data = await CommentRepository.GetComments().AsNoTracking()
                         .Where(x => x.AreaId == id && x.NewsFeedTypeId == (int)newsFeedTypeId)
                         .OrderByDescending(x => x.CreatedDate)
                         .Select(CommentRepository.SelectCommand).ToListAsync().ConfigureAwait(false);

            data.ForEach(x => x.UserProfileUrl = userImageFinder.FindImage(x.UserIdThatMadeComment));

            return data;
        }

        public async Task<int> CommentLikeAddOrRemove(ToracGolfContext dbContext, int userId, int commentId)
        {
            //go take care of the like
            await LikeRepository.AddOrRemoveLike(commentId, NewsFeedItemModel.NewsFeedTypeId.Comment, userId).ConfigureAwait(false);

            //return the new count
            return await LikeRepository.GetLikes().AsNoTracking().CountAsync(x => x.AreaId == commentId && x.NewsFeedTypeId == (int)NewsFeedItemModel.NewsFeedTypeId.Comment).ConfigureAwait(false);
        }

        #endregion

    }

}
