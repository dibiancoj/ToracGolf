using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ToracGolf.MiddleLayer.EFModel.Tables
{
    public class CourseTeeLocations
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CourseTeeLocationId { get; set; }

        [Required]
        public int CourseId { get; set; }

        [Required]
        [StringLength(50)]
        public string Description { get; set; }

        [Required]
        public int TeeLocationSortOrderId { get; set; }

        [Required]
        public int Yardage { get; set; }

        [Required]
        public int Front9Par { get; set; }

        [Required]
        public int Back9Par { get; set; }

        [Required]
        public double Rating { get; set; }

        [Required]
        public double Slope { get; set; }

    }
}
