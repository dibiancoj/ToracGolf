using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ToracGolf.MiddleLayer.EFModel.Tables
{
    public class Ref_Season
    {

        public int SeasonId { get; set; }

        [Required]
        [StringLength(50)]
        public string SeasonText { get; set; }

        [Required]
        public DateTime CreatedDate { get; set; }

    }
}
