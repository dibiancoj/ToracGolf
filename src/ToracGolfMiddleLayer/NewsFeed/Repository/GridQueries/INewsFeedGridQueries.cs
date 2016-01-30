using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToracGolf.MiddleLayer.Courses;
using ToracGolf.MiddleLayer.NewsFeed.Models;

namespace ToracGolf.MiddleLayer.NewsFeed.Repository.GridQueries
{
    public interface INewsFeedGridQueries
    {
        Task<NewsFeedQueryResult> NewsFeedPostSelect(int userId, NewsFeedItemModel.NewsFeedTypeId? newsFeedTypeIdFilter, string searchFilterText, ImageFinder courseImageFinder);
    }

}
