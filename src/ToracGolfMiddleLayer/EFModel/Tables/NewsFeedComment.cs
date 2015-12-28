using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ToracGolf.MiddleLayer.EFModel.Tables
{

    [Table("NewsFeedComment")]
    public class NewsFeedComment
    {

        [Column(Order = 1), Key]
        public int NewsFeedTypeId { get; set; }

        [Column(Order = 2), Key]
        public int AreaId { get; set; }

        [Column(Order = 3), Key]
        public int UserIdThatCommented { get; set; }

        public string Comment { get; set; }

        public DateTime CreatedDate { get; set; }

    }
}
