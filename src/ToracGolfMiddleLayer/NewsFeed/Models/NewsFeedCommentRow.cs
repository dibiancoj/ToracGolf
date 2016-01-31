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

        public DateTime CommentDate { get; set; }

        public int UserIdThatMadeComment { get; set; }

        public string UserProfileUrl { get; set; }

        //public bool UserLikesThisComment { get; set; }

    }
}
