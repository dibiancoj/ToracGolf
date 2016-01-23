using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using ToracGolf.MiddleLayer.EFModel.Tables;

namespace ToracGolf.MiddleLayer.NewsFeed.Repository.Likes
{
    public interface INewsFeedLikeRepository
    {
        //Expression<Func<NewsFeedComment, NewsFeedCommentRow>> SelectCommand { get; }

        IQueryable<NewsFeedLike> GetLikes();

        Task<NewsFeedLike> GetLikesById(int areaId, NewsFeedItemModel.NewsFeedTypeId newsFeedTypeId);

        Task<NewsFeedLike> GetLikesByIdAndUserId(int areaId, NewsFeedItemModel.NewsFeedTypeId newsFeedTypeId, int userId);

        Task<bool> AddOrRemoveLike(int areaId, NewsFeedItemModel.NewsFeedTypeId newsFeedTypeId, int userId);

        Task<bool> Add(IEnumerable<NewsFeedLike> recordsToAdd);

        Task<bool> Add(NewsFeedLike recordToAdd);

        Task<bool> Delete(NewsFeedLike recordToDelete);

        Task<bool> Delete(Expression<Func<NewsFeedLike, bool>> deleteRecords);

        Task<bool> Delete(int areaId, NewsFeedItemModel.NewsFeedTypeId newsFeedTypeId);

    }
}
