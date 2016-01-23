using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using ToracGolf.MiddleLayer.EFModel;
using ToracGolf.MiddleLayer.EFModel.Tables;

namespace ToracGolf.MiddleLayer.NewsFeed.Repository.Likes
{

    public class NewsFeedLikeRepository : INewsFeedLikeRepository
    {

        #region Constructor

        public NewsFeedLikeRepository(ToracGolfContext dbContext)
        {
            DbContext = dbContext;
        }

        #endregion

        #region Immutable Properties

        public ToracGolfContext DbContext { get; }

        #endregion

        #region Repository

        public IQueryable<NewsFeedLike> GetLikes()
        {
            //start the query
            return DbContext.NewsFeedLike.AsQueryable();
        }

        public async Task<NewsFeedLike> GetLikesById(int areaId, NewsFeedItemModel.NewsFeedTypeId newsFeedTypeId)
        {
            return await GetLikes().Where(x => x.AreaId == areaId && x.NewsFeedTypeId == (int)newsFeedTypeId).FirstOrDefaultAsync();
        }

        public async Task<NewsFeedLike> GetLikesByIdAndUserId(int areaId, NewsFeedItemModel.NewsFeedTypeId newsFeedTypeId, int userId)
        {
            return await GetLikes().Where(x => x.UserIdThatLikedItem == userId && x.AreaId == areaId && x.NewsFeedTypeId == (int)newsFeedTypeId).FirstOrDefaultAsync();
        }

        #region Delete

        public async Task<bool> Delete(NewsFeedLike recordToDelete)
        {
            return await DeleteRecordHelper(new NewsFeedLike[] { recordToDelete });
        }

        public async Task<bool> Delete(int areaId, NewsFeedItemModel.NewsFeedTypeId newsFeedTypeId)
        {
            return await Delete(x => x.AreaId == areaId && x.NewsFeedTypeId == (int)newsFeedTypeId);
        }

        public async Task<bool> Delete(Expression<Func<NewsFeedLike, bool>> deleteRecords)
        {
            return await DeleteRecordHelper(await DbContext.NewsFeedLike.Where(deleteRecords).ToArrayAsync());
        }

        private async Task<bool> DeleteRecordHelper(IEnumerable<NewsFeedLike> recordToDelete)
        {
            DbContext.NewsFeedLike.RemoveRange(recordToDelete);

            await DbContext.SaveChangesAsync();

            return true;
        }

        #endregion

        #region Add

        public async Task<bool> Add(IEnumerable<NewsFeedLike> recordsToAdd)
        {
            DbContext.NewsFeedLike.AddRange(recordsToAdd);

            await DbContext.SaveChangesAsync();

            return true;
        }

        public async Task<bool> Add(NewsFeedLike recordToAdd)
        {
            return await Add(new NewsFeedLike[] { recordToAdd });
        }

        #endregion

        #endregion

    }

}
