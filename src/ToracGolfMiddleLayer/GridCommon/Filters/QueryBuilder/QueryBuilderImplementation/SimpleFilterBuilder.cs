using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using ToracGolf.MiddleLayer.EFModel;
using ToracLibrary.Core.ExpressionTrees.API;

namespace ToracGolf.MiddleLayer.GridCommon.Filters.QueryBuilder
{

    public class SimpleFilterBuilder<TQueryType> : IQueryBuilder<TQueryType>
            where TQueryType : class
    {

        #region Constructor

        public SimpleFilterBuilder(FilterConfig<TQueryType> filterConfig)
        {
            FilterConfig = filterConfig;
        }

        #endregion

        #region Properties

        public FilterConfig<TQueryType> FilterConfig { get; }

        #endregion

        #region Methods

        public IQueryable<TQueryType> BuildAFilterQuery(ToracGolfContext dbContext, IQueryable<TQueryType> query, KeyValuePair<string, object> filter)
        {
            Expression<Func<TQueryType, bool>> expressionToAdd;

            //string contains vs everything else
            if (FilterConfig.Parameter.PropertyMemberExpression.Type == typeof(string))
            {
                expressionToAdd = Expression.Lambda<Func<TQueryType, bool>>(Expression.Call(FilterConfig.Parameter.PropertyMemberExpression, typeof(string).GetMethod(nameof(string.Contains)), Expression.Constant(filter.Value.ToString(), typeof(string))), FilterConfig.Parameter.ParametersForExpression);
            }
            else
            {
                expressionToAdd = ExpressionBuilder.BuildStatement<TQueryType>(FilterConfig.Parameter, FilterConfig.FilterOperation.Value, filter.Value);
            }

            //tack it on to the query
            return query.Where(expressionToAdd);
        }

        #endregion

    }

}
