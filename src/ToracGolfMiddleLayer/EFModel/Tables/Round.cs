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

        [Required]
        public double RoundHandicap { get; set; }

        public int? GreensInRegulation { get; set; }
        public int? FairwaysHit { get; set; }
        public int? Putts { get; set; }

        public virtual Course Course { get; set; }

        public virtual CourseTeeLocations CourseTeeLocation { get; set; }

        public virtual Handicap Handicap { get; set; }

        public virtual UserAccounts User { get; set; }

    }
}
