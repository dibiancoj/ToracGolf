using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ToracGolf.MiddleLayer.NewsFeed.Models
{
    public class NewsFeedQueryResult
    {

        public NewsFeedQueryResult(IEnumerable<NewsFeedItemModel> results, int unFilteredRoundCount, int unFilteredCourseCount, int friendCount)
        {
            Results = results;
            UnFilteredCourseCount = unFilteredCourseCount;
            UnFilteredRoundCount = unFilteredRoundCount;
            FriendCount = friendCount;
        }

        public IEnumerable<NewsFeedItemModel> Results { get; }
        public int UnFilteredRoundCount { get; }
        public int UnFilteredCourseCount { get; }
        public int FriendCount { get; set; }

    }
}
