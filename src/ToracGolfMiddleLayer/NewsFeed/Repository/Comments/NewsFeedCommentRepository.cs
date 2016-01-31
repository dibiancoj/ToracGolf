using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using ToracGolf.MiddleLayer.EFModel;
using ToracGolf.MiddleLayer.EFModel.Tables;
using ToracGolf.MiddleLayer.NewsFeed.Models;

namespace ToracGolf.MiddleLayer.NewsFeed.Repository.Comments
{

    public class NewsFeedCommentRepository : INewsFeedCommentRepository
    {

        #region Constructor

        public NewsFeedCommentRepository(ToracGolfContext dbContext)
        {
            DbContext = dbContext;

            //just set the method....we are doing this so the calling user doesn't have to populate it in a variable first
            SelectCommand = SelectCommentsBuilder();

            //UserProfileFinder = userProfileFinder;
        }

        #endregion

        #region Immutable Properties

        public ToracGolfContext DbContext { get; }

        public Expression<Func<NewsFeedComment, NewsFeedCommentRow>> SelectCommand { get; }

        public ImageFinder UserProfileFinder { get; }

        #endregion

        #region Repository

        public IQueryable<NewsFeedComment> GetComments()
        {
            //start the query
            return DbContext.NewsFeedComment.AsQueryable();
        }

        public async Task<NewsFeedCommentRow> GetCommentById(int commentId)
        {
            return await GetComments().Where(x => x.CommentId == commentId).Select(SelectCommand).FirstOrDefaultAsync();
        }

        private Expression<Func<NewsFeedComment, NewsFeedCommentRow>> SelectCommentsBuilder()
        {
            return x => new NewsFeedCommentRow
            {
                CommentId = x.CommentId,
                User = x.User.FirstName + " " + x.User.LastName,
                CommentText = x.Comment,
                NumberOfLikes = DbContext.NewsFeedLike.Count(y => y.AreaId == x.CommentId && y.NewsFeedTypeId == (int)NewsFeedItemModel.NewsFeedTypeId.Comment),
                CommentDate = x.CreatedDate,
                UserIdThatMadeComment = x.UserIdThatCommented
                //UserProfileUrl = UserProfileFinder.FindImage(x.UserIdThatCommented)
            };
        }

        #region Delete 

        public async Task<bool> Delete(Expression<Func<NewsFeedComment, bool>> deleteRecords)
        {
            return await DeleteRecordHelper(await DbContext.NewsFeedComment.Where(deleteRecords).ToArrayAsync());
        }

        public async Task<bool> Delete(NewsFeedComment recordToDelete)
        {
            return await DeleteRecordHelper(new NewsFeedComment[] { recordToDelete });
        }

        public async Task<bool> Delete(int commentId)
        {
            return await Delete(x => x.CommentId == commentId);
        }

        private async Task<bool> DeleteRecordHelper(IEnumerable<NewsFeedComment> recordToDelete)
        {
            DbContext.NewsFeedComment.RemoveRange(recordToDelete);

            await DbContext.SaveChangesAsync();

            return true;
        }

        #endregion

        #region Add

        public async Task<bool> Add(IEnumerable<NewsFeedComment> recordsToAdd)
        {
            DbContext.NewsFeedComment.AddRange(recordsToAdd);

            await DbContext.SaveChangesAsync();

            return true;
        }

        public async Task<bool> Add(NewsFeedComment recordToAdd)
        {
            return await Add(new NewsFeedComment[] { recordToAdd });
        }

        #endregion

        #endregion

    }

}
