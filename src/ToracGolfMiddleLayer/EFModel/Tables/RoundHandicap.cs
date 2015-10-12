using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ToracGolf.MiddleLayer.EFModel.Tables
{

    public class RoundHandicap
    {

        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int RoundId { get; set; }

        public double HandicapBeforeRound { get; set; }

    }

}
