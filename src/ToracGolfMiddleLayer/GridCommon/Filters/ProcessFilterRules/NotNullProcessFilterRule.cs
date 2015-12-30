using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ToracGolf.MiddleLayer.GridCommon.Filters.ProcessFilterRules
{
    public class NotNullProcessFilterRule : IProcessFilterRule
    {
        public bool ProcessFilter(KeyValuePair<string, object> filterValue)
        {
            return filterValue.Value != null;
        }
    }
}
