using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ToracGolf.MiddleLayer.EFModel.Tables
{
    public class UserSeason
    {

     
        public int UserId { get; set; }

        public int SeasonId { get; set; }

        [Required]
        public DateTime CreatedDate { get; set; }

    }
}
