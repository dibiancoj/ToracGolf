using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ToracGolf.MiddleLayer.EFModel.Tables
{
    public class Friends
    {

        [Required]
        public int UserId { get; set; }

        [Required]
        public int FriendId { get; set; }

        [Required]
        public DateTime CreatedDate { get; set; }

    }
}
