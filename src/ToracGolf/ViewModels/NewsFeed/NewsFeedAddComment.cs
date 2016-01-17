using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToracGolf.MiddleLayer.NewsFeed;

namespace ToracGolf.ViewModels.NewsFeed
{
    public class NewsFeedAddComment : NewsFeedAddLike
    {
        public string CommentToAdd { get; set; }
    }
}
