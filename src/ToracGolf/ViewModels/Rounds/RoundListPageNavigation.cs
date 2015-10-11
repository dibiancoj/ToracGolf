﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToracGolf.MiddleLayer.Rounds.Models;

namespace ToracGolf.ViewModels.Rounds
{

    public class RoundListPageNavigation
    {

        #region Properties

        public bool ResetPager { get; set; }

        public int PageIndexId { get; set; }

        public RoundListingSortOrder.RoundListingSortEnum SortBy { get; set; }

        public string CourseNameFilter { get; set; }

        public string SeasonFilter { get; set; }

        public int RoundsPerPage { get; set; }

        #endregion

    }

}