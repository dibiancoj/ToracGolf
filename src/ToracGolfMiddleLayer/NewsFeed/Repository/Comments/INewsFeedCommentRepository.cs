using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using ToracGolf.MiddleLayer.EFModel.Tables;
using ToracGolf.MiddleLayer.NewsFeed.Models;

namespace ToracGolf.MiddleLayer.NewsFeed.Repository.Comments
{
    public interface INewsFeedCommentRepository
    {
        Expression<Func<NewsFeedComment, NewsFeedCommentRow>> SelectCommand { get; }

        IQueryable<NewsFeedComment> GetComments();

        Task<NewsFeedCommentRow> GetCommentById(int commentId);

        Task<bool> Add(IEnumerable<NewsFeedComment> recordsToAdd);

        Task<bool> Add(NewsFeedComment recordToAdd);

        Task<bool> Delete(NewsFeedComment recordToDelete);

        Task<bool> Delete(Expression<Func<NewsFeedComment, bool>> deleteRecords);

        Task<bool> Delete(int commentId);

    }
}
