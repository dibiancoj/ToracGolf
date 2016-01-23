using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ToracGolf.MiddleLayer.EFModel.Tables
{

    public class NewsFeedComment
    {

        public int CommentId { get; set; }

        public int NewsFeedTypeId { get; set; }

        public int AreaId { get; set; }

        public int UserIdThatCommented { get; set; }

        public string Comment { get; set; }

        public DateTime CreatedDate { get; set; }

        public UserAccounts User { get; set; }

    }
}
