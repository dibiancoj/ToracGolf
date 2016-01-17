using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ToracGolf.MiddleLayer.NewsFeed.Models
{
    public class NewsFeedCommentRow
    {

        public NewsFeedCommentRow(string user, string commentText)
        {
            User = user;
            CommentText = commentText;
        }

        public string User { get; set; }
        public string CommentText { get; set; }

    }
}
