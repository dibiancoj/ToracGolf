using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using ToracGolf.MiddleLayer.EFModel;
using ToracLibrary.Core.ExpressionTrees.API;

namespace ToracGolf.MiddleLayer.GridCommon.Filters.QueryBuilder
{

    public class SimpleFilterBuilder : IQueryBuilder
    {

        #region Constructor

        public SimpleFilterBuilder(FilterConfig filterConfig)
        {
            FilterConfig = filterConfig;
        }

        #endregion

        #region Properties

        public FilterConfig FilterConfig { get; }

        #endregion

        #region Methods

        public IQueryable<TFrom> BuildAFilterQuery<TFrom, TTo>(ToracGolfContext dbContext, IQueryable<TFrom> query, KeyValuePair<string, object> filter)
             where TFrom : class
             where TTo : class
        {
            //build the expression
            var expression = CreateProperty<TFrom>(FilterConfig, filter.Value);

            //tack it on to the query
            return query.Where(expression);
        }

        private static Expression<Func<TFrom, bool>> CreateProperty<TFrom>(FilterConfig filterConfig, object valueToQuery)
            where TFrom : class
        {
            if (filterConfig.Parameter.PropertyMemberExpression.Type == typeof(int?))
            {
                return ExpressionBuilder.BuildStatement<TFrom, int?>(filterConfig.Parameter, filterConfig.FilterOperation.Value, (int?)valueToQuery);
            }

            if (filterConfig.Parameter.PropertyMemberExpression.Type == typeof(int))
            {
                return ExpressionBuilder.BuildStatement<TFrom, int>(filterConfig.Parameter, filterConfig.FilterOperation.Value, (int)valueToQuery);
            }

            if (filterConfig.Parameter.PropertyMemberExpression.Type == typeof(DateTime))
            {
                return ExpressionBuilder.BuildStatement<TFrom, DateTime>(filterConfig.Parameter, filterConfig.FilterOperation.Value, (DateTime)valueToQuery);
            }

            if (filterConfig.Parameter.PropertyMemberExpression.Type == typeof(DateTime?))
            {
                return ExpressionBuilder.BuildStatement<TFrom, DateTime?>(filterConfig.Parameter, filterConfig.FilterOperation.Value, (DateTime?)valueToQuery);
            }

            if (filterConfig.Parameter.PropertyMemberExpression.Type == typeof(string))
            {
                return Expression.Lambda<Func<TFrom, bool>>(Expression.Call(filterConfig.Parameter.PropertyMemberExpression, typeof(string).GetMethod(nameof(string.Contains)), Expression.Constant(valueToQuery.ToString(), typeof(string))), filterConfig.Parameter.ParametersForExpression);
            }

            throw new NotImplementedException();
        }

        #endregion

    }

}
