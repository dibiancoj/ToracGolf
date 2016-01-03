using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToracGolf.MiddleLayer.NewsFeed;

namespace ToracGolf.ViewModels.NewsFeed
{
    public class NewsFeedAddLike
    {
        public int Id { get; set; }
        public NewsFeedItemModel.NewsFeedTypeId NewsFeedTypeId { get; set; }
    }
}
