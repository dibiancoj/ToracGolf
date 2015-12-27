using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToracGolf.MiddleLayer.EFModel;

namespace ToracGolf.MiddleLayer.NewsFeed
{

    public static class NewsFeedDataProvider
    {

        public static async Task<IEnumerable<NewsFeedItemModel>> NewsFeedPostSelect(ToracGolfContext dbContext, int userId)
        {
            return await Task.FromResult<IEnumerable<NewsFeedItemModel>>(new List<NewsFeedItemModel>()
            {
                new NewsFeedItemModel { PostDate = DateTime.Now },
                new NewsFeedItemModel { PostDate = DateTime.Now.AddMonths(-1) }
            });
        }

    }

}
