﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToracGolf.MiddleLayer.NewsFeed.Models;

namespace ToracGolf.MiddleLayer.NewsFeed
{
    public abstract class NewsFeedItemModel
    {

        public NewsFeedItemModel()
        {
            // Comments = Enumerable.Empty<NewsFeedCommentRow>();
            Comments = new List<NewsFeedCommentRow>()
           {
               new NewsFeedCommentRow ("jason dibianco","NewsFeedItemModel in constructor...to test comment row...remove when done")
           };
        }

        public enum NewsFeedTypeId
        {
            NewRound = 0,
            NewCourse = 1
            //what else?
        }

        public int Id { get; set; }

        public abstract NewsFeedTypeId FeedTypeId { get; }

        public string TitleOfPost { get; set; }

        public IEnumerable<string> BodyOfPost { get; set; }

        public DateTime PostDate { get; set; }

        public string CourseImagePath { get; set; }

        public int CommentCount { get; set; }

        public int LikeCount { get; set; }

        public bool YouLikedItem { get; set; }

        public IEnumerable<NewsFeedCommentRow> Comments { get; set; }

    }

    public class NewRoundNewsFeed : NewsFeedItemModel
    {

        public override NewsFeedTypeId FeedTypeId
        {
            get
            {
                return NewsFeedTypeId.NewRound;
            }
        }

    }

    public class NewCourseNewsFeed : NewsFeedItemModel
    {

        public override NewsFeedTypeId FeedTypeId
        {
            get
            {
                return NewsFeedTypeId.NewCourse;
            }
        }

    }

}
