using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToracGolf.MiddleLayer.NewsFeed;

namespace ToracGolf.ViewModels.NewsFeed
{
    public class NewsFeedGetRequest
    {
        public NewsFeedItemModel.NewsFeedTypeId? NewsFeedTypeIdFilter { get; set; }
    }
}
