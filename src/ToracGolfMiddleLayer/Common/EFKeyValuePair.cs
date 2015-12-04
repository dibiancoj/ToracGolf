using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ToracGolf.MiddleLayer.Common
{

    /// <summary>
    /// Class where we can do a select with EF because we don't have parameters in the constructor
    /// </summary>
    public class EFKeyValuePair
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }

}
