using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToracGolf.MiddleLayer.EFModel;

namespace ToracGolf.MiddleLayer.Friend
{
    public static class FriendsDataProvider
    {

        public static IQueryable<int> GetUsersFriendList(ToracGolfContext dbContext, int userIdToGetFriendsOf)
        {
            return dbContext.Friends.AsNoTracking().Where(x => x.UserId == userIdToGetFriendsOf).Select(x => x.FriendId);
        }

    }
}
