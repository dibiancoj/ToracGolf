using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ToracGolf.MiddleLayer.NewsFeed.Models
{
    public class NewsFeedCommentRow
    {

        public NewsFeedCommentRow(int commentId, string user, string commentText, int numberOfLikes)//, bool userLikesThisComment)
        {
            CommentId = commentId;
            User = user;
            CommentText = commentText;
            NumberOfLikes = numberOfLikes;
            //UserLikesThisComment = userLikesThisComment;
        }

        public int CommentId { get; set; }
        public string User { get; set; }
        public string CommentText { get; set; }

        public int NumberOfLikes { get; set; }

        //public bool UserLikesThisComment { get; set; }

    }
}
