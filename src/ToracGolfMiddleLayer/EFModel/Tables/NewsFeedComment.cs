using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ToracGolf.MiddleLayer.EFModel.Tables
{

    public class NewsFeedComment
    {
  
        public int NewsFeedTypeId { get; set; }

        public int AreaId { get; set; }

        public int UserIdThatCommented { get; set; }

        public string Comment { get; set; }

        public DateTime CreatedDate { get; set; }

    }
}
