using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ToracGolf.MiddleLayer.GridCommon
{
    public class SortOrderViewModel
    {
        public SortOrderViewModel(int sortOrder, string value, string description)
        {
            SortOrder = sortOrder;
            Value = value;
            Description = description;
        }

        public int SortOrder { get; }
        public string Value { get; }
        public string Description { get; }

    }
}
