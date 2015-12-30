using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToracGolf.MiddleLayer.GridCommon.Filters.QueryBuilder;
using ToracLibrary.Core.ExpressionTrees.API;
using static ToracLibrary.Core.ExpressionTrees.API.ExpressionBuilder;

namespace ToracGolf.MiddleLayer.GridCommon.Filters
{
    public class FilterConfig
    {

        public FilterConfig(ParameterBuilderResults parameter, DynamicUtilitiesEquations? filterOperation)
        {
            Parameter = parameter;
            FilterOperation = filterOperation;
        }

        public ParameterBuilderResults Parameter { get; }

        public DynamicUtilitiesEquations? FilterOperation { get; }

    }
}
