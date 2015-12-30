using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToracGolf.MiddleLayer.EFModel;

namespace ToracGolf.MiddleLayer.ListingFactories
{
    public class ListingFactoryParameters
    {

        public ListingFactoryParameters(ToracGolfContext dbContext, int userId)
        {
            DbContext = dbContext;
            UserId = userId;
        }

        public ToracGolfContext DbContext { get; }
        public int UserId { get; }
    }
}
