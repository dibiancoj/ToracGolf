using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using ToracLibrary.Core.ExpressionTrees.API;
using static ToracLibrary.Core.ExpressionTrees.API.ExpressionBuilder;

namespace ToracGolf.MiddleLayer.GridCommon.Filters
{

    public class FilterConfig<TQueryType>
        where TQueryType : class
    {

        #region Constructor Overloads

        public FilterConfig(Expression<Func<TQueryType, string>> PropertySelector)
        {
            Parameter = ParameterBuilder.BuildParameterFromLinqPropertySelector(PropertySelector);
        }

        public FilterConfig(Expression<Func<TQueryType, int>> PropertySelector, DynamicUtilitiesEquations? filterOperation)
        {
            Parameter = ParameterBuilder.BuildParameterFromLinqPropertySelector(PropertySelector);
            FilterOperation = filterOperation;
        }

        public FilterConfig(Expression<Func<TQueryType, int?>> PropertySelector, DynamicUtilitiesEquations? filterOperation)
        {
            Parameter = ParameterBuilder.BuildParameterFromLinqPropertySelector(PropertySelector);
            FilterOperation = filterOperation;
        }

        public FilterConfig(Expression<Func<TQueryType, bool?>> PropertySelector, DynamicUtilitiesEquations? filterOperation)
        {
            Parameter = ParameterBuilder.BuildParameterFromLinqPropertySelector(PropertySelector);
            FilterOperation = filterOperation;
        }

        public FilterConfig(Expression<Func<TQueryType, bool>> PropertySelector, DynamicUtilitiesEquations? filterOperation)
        {
            Parameter = ParameterBuilder.BuildParameterFromLinqPropertySelector(PropertySelector);
            FilterOperation = filterOperation;
        }

        public FilterConfig(Expression<Func<TQueryType, DateTime?>> PropertySelector, DynamicUtilitiesEquations? filterOperation)
        {
            Parameter = ParameterBuilder.BuildParameterFromLinqPropertySelector(PropertySelector);
            FilterOperation = filterOperation;
        }

        public FilterConfig(Expression<Func<TQueryType, DateTime>> PropertySelector, DynamicUtilitiesEquations? filterOperation)
        {
            Parameter = ParameterBuilder.BuildParameterFromLinqPropertySelector(PropertySelector);
            FilterOperation = filterOperation;
        }

        #endregion

        #region Properties

        public ParameterBuilderResults Parameter { get; }

        public DynamicUtilitiesEquations? FilterOperation { get; }

        #endregion

    }

}
