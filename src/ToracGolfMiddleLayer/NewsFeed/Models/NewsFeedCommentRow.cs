using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ToracGolf.MiddleLayer.NewsFeed.Models
{
    public class NewsFeedCommentRow
    {

        public int CommentId { get; set; }
        public string User { get; set; }
        public string CommentText { get; set; }

        public int NumberOfLikes { get; set; }

        //public bool UserLikesThisComment { get; set; }

    }
}
