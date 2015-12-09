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

        [Key, Column(Order = 0)]
        public int UserId { get; set; }

        [Key, Column(Order = 1)]
        public int SeasonId { get; set; }

        [Required]
        public DateTime CreatedDate { get; set; }

        public ICollection<Round> Rounds { get; set; } 

    }
}
