using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ToracGolf.MiddleLayer.GridCommon.Filters.ProcessFilterRules
{
    public class StringIsNotNullProcessFilterRule : IProcessFilterRule
    {
        public bool ProcessFilter(KeyValuePair<string, object> filterValue)
        {
            return filterValue.Value != null && !string.IsNullOrEmpty(filterValue.Value.ToString());
        }
    }
}
