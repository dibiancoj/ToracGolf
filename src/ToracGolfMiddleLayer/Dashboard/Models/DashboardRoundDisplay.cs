using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ToracGolf.MiddleLayer.Dashboard.Models
{

    public class DashboardRoundDisplay
    {

        #region Properties

        public int CourseId { get; set; }
        public string CourseName { get; set; }
        public DateTime RoundDate { get; set; }
        public string TeeBoxLocation { get; set; }
        public int Score { get; set; }

        #endregion

    }

}
