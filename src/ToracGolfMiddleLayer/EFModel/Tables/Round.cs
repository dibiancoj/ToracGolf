using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ToracGolf.MiddleLayer.EFModel.Tables
{
    public class Round
    {

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RoundId { get; set; }

        [Required]
        public int CourseId { get; set; }

        [Required]
        public int CourseTeeLocationId { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public int SeasonId { get; set; }

        [Required]
        public DateTime RoundDate { get; set; }

        [Required]
        public int Score { get; set; }

        [Required]
        public bool Is9HoleScore { get; set; }

    }
}
