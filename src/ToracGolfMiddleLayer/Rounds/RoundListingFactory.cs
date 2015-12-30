using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToracGolf.MiddleLayer.EFModel.Tables;
using ToracGolf.MiddleLayer.GridCommon.Filters;
using ToracGolf.MiddleLayer.GridCommon.Filters.QueryBuilder;
using ToracGolf.MiddleLayer.ListingFactories;
using ToracGolf.MiddleLayer.Rounds.Filters;
using ToracGolf.MiddleLayer.Rounds.Models;
using ToracLibrary.Core.ExpressionTrees.API;
using static ToracLibrary.Core.ExpressionTrees.API.ExpressionBuilder;

namespace ToracGolf.MiddleLayer.Rounds
{
    public class RoundListingFactory : IListingFactory<Round, RoundListingData>
    {

        #region Constructor

        public RoundListingFactory(IDictionary<string, Func<IQueryable<Round>, ListingFactoryParameters, IOrderedQueryable<Round>>> sortByConfiguration,
                                   IDictionary<string, IQueryBuilder> filterConfiguration)
        {
            SortByConfiguration = sortByConfiguration;
            FilterConfiguration = filterConfiguration;
        }

        #endregion

        #region Properties

        public IDictionary<string, Func<IQueryable<Round>, ListingFactoryParameters, IOrderedQueryable<Round>>> SortByConfiguration { get; }

        public IDictionary<string, IQueryBuilder> FilterConfiguration { get; }

        #endregion

        #region Lookup

        public static IDictionary<string, Func<IQueryable<Round>, ListingFactoryParameters, IOrderedQueryable<Round>>> SortByConfigurationBuilder()
        {
            var dct = new Dictionary<string, Func<IQueryable<Round>, ListingFactoryParameters, IOrderedQueryable<Round>>>();

            dct.Add(RoundListingSortOrder.RoundListingSortEnum.CourseNameAscending.ToString(), (x, param) => x.OrderBy(y => y.Course.Name));
            dct.Add(RoundListingSortOrder.RoundListingSortEnum.CourseNameDescending.ToString(), (x, param) => x.OrderByDescending(y => y.Course.Name));

            dct.Add(RoundListingSortOrder.RoundListingSortEnum.RoundDateAscending.ToString(), (x, param) => x.OrderBy(y => y.RoundDate));
            dct.Add(RoundListingSortOrder.RoundListingSortEnum.RoundDateDescending.ToString(), (x, param) => x.OrderByDescending(y => y.RoundDate));

            dct.Add(RoundListingSortOrder.RoundListingSortEnum.BestRawScore.ToString(), (x, param) => x.OrderBy(y => y.Score));
            dct.Add(RoundListingSortOrder.RoundListingSortEnum.WorseRawScore.ToString(), (x, param) => x.OrderByDescending(y => y.Score));

            dct.Add(RoundListingSortOrder.RoundListingSortEnum.RoundHandicapAscending.ToString(), (x, param) => x.OrderBy(y => y.RoundHandicap));
            dct.Add(RoundListingSortOrder.RoundListingSortEnum.RoundHandicapDescending.ToString(), (x, param) => x.OrderByDescending(y => y.RoundHandicap));

            return dct;
        }

        public static IDictionary<string, IQueryBuilder> FilterByConfigurationBuilder()
        {
            var dct = new Dictionary<string, IQueryBuilder>();

            dct.Add("courseNameFilter", new SimpleFilterBuilder(new FilterConfig(ParameterBuilder.BuildParameterFromLinqPropertySelector<Round>(x => x.Course.Name), null)));
            dct.Add("seasonFilter", new SimpleFilterBuilder(new FilterConfig(ParameterBuilder.BuildParameterFromLinqPropertySelector<Round>(x => x.SeasonId), DynamicUtilitiesEquations.Equal)));
            dct.Add("roundDateStartFilter", new SimpleFilterBuilder(new FilterConfig(ParameterBuilder.BuildParameterFromLinqPropertySelector<Round>(x => x.RoundDate), DynamicUtilitiesEquations.GreaterThanOrEqual)));
            dct.Add("roundDateEndFilter", new SimpleFilterBuilder(new FilterConfig(ParameterBuilder.BuildParameterFromLinqPropertySelector<Round>(x => x.RoundDate), DynamicUtilitiesEquations.LessThanOrEqual)));
            dct.Add("handicappedRoundOnly", new HandicapRoundsOnlyFilter());

            return dct;
        }

        #endregion

    }
}
