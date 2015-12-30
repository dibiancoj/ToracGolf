using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ToracGolf.MiddleLayer.GridCommon.Filters.ProcessFilterRules
{
    public interface IProcessFilterRule
    {
        bool ProcessFilter(KeyValuePair<string, object> filterValue);
    }
}
