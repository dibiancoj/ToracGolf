﻿using System;
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
            Expression<Func<TFrom, bool>> expressionToAdd;

            //string contains vs everything else
            if (FilterConfig.Parameter.PropertyMemberExpression.Type == typeof(string))
            {
                expressionToAdd = Expression.Lambda<Func<TFrom, bool>>(Expression.Call(FilterConfig.Parameter.PropertyMemberExpression, typeof(string).GetMethod(nameof(string.Contains)), Expression.Constant(filter.Value.ToString(), typeof(string))), FilterConfig.Parameter.ParametersForExpression);
            }
            else
            {
                expressionToAdd = ExpressionBuilder.BuildStatement<TFrom>(FilterConfig.Parameter, FilterConfig.FilterOperation.Value, filter.Value);
            }

            //tack it on to the query
            return query.Where(expressionToAdd);
        }

        #endregion

    }

}
