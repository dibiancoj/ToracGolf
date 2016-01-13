using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ToracGolf.MiddleLayer.EFModel.Tables
{

    public class NewsFeedLike
    {

        public int NewsFeedTypeId { get; set; }

        public int AreaId { get; set; }

        public int UserIdThatLikedItem { get; set; }

        public DateTime CreatedDate { get; set; }

    }
}
