using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ToracGolf.MiddleLayer.EFModel.Tables
{
    public class Ref_Season
    {

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SeasonId { get; set; }

        [Required]
        [StringLength(50)]
        public string SeasonText { get; set; }

        [Required]
        public DateTime CreatedDate { get; set; }

    }
}
