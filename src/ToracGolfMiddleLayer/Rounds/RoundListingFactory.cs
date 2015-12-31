using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using ToracGolf.MiddleLayer.EFModel.Tables;
using ToracGolf.MiddleLayer.GridCommon.Filters;
using ToracGolf.MiddleLayer.GridCommon.Filters.ProcessFilterRules;
using ToracGolf.MiddleLayer.GridCommon.Filters.QueryBuilder;
using ToracGolf.MiddleLayer.ListingFactories;
using ToracGolf.MiddleLayer.Rounds.Filters;
using ToracGolf.MiddleLayer.Rounds.Models;
using static ToracLibrary.Core.ExpressionTrees.API.ExpressionBuilder;

namespace ToracGolf.MiddleLayer.Rounds
{
    public class RoundListingFactory : IListingFactory<RoundListingFactory.RoundListingSortEnum, Round, RoundListingData>
    {

        #region Constructor

        public RoundListingFactory(IDictionary<RoundListingSortEnum, Func<IQueryable<Round>, ListingFactoryParameters, IOrderedQueryable<Round>>> sortByConfiguration,
                                   IDictionary<string, IQueryBuilder<Round>> filterConfiguration)
        {
            SortByConfiguration = sortByConfiguration;
            FilterConfiguration = filterConfiguration;
        }

        #endregion

        #region Properties

        public IDictionary<RoundListingSortEnum, Func<IQueryable<Round>, ListingFactoryParameters, IOrderedQueryable<Round>>> SortByConfiguration { get; }

        public IDictionary<string, IQueryBuilder<Round>> FilterConfiguration { get; }

        #endregion

        #region Sort Enum

        public enum RoundListingSortEnum
        {

            [Description("Round Date Descending")]
            RoundDateDescending = 0,

            [Description("Round Date Ascending")]
            RoundDateAscending = 1,

            [Description("Course Name Descending")]
            CourseNameDescending = 2,

            [Description("Course Name Ascending")]
            CourseNameAscending = 3,

            [Description("Best Raw Score")]
            BestRawScore = 4,

            [Description("Worse Raw Score")]
            WorseRawScore = 5,

            [Description("Round Handicap Descending")]
            RoundHandicapDescending = 6,

            [Description("Round Handicap Ascending")]
            RoundHandicapAscending = 7,

        }

        #endregion

        #region Lookup

        public static IDictionary<RoundListingSortEnum, Func<IQueryable<Round>, ListingFactoryParameters, IOrderedQueryable<Round>>> SortByConfigurationBuilder()
        {
            var dct = new Dictionary<RoundListingSortEnum, Func<IQueryable<Round>, ListingFactoryParameters, IOrderedQueryable<Round>>>();

            dct.Add(RoundListingSortEnum.CourseNameAscending, (x, param) => x.OrderBy(y => y.Course.Name));
            dct.Add(RoundListingSortEnum.CourseNameDescending, (x, param) => x.OrderByDescending(y => y.Course.Name));

            dct.Add(RoundListingSortEnum.RoundDateAscending, (x, param) => x.OrderBy(y => y.RoundDate));
            dct.Add(RoundListingSortEnum.RoundDateDescending, (x, param) => x.OrderByDescending(y => y.RoundDate));

            dct.Add(RoundListingSortEnum.BestRawScore, (x, param) => x.OrderBy(y => y.Score));
            dct.Add(RoundListingSortEnum.WorseRawScore, (x, param) => x.OrderByDescending(y => y.Score));

            dct.Add(RoundListingSortEnum.RoundHandicapAscending, (x, param) => x.OrderBy(y => y.RoundHandicap));
            dct.Add(RoundListingSortEnum.RoundHandicapDescending, (x, param) => x.OrderByDescending(y => y.RoundHandicap));

            return dct;
        }

        public static IDictionary<string, IQueryBuilder<Round>> FilterByConfigurationBuilder()
        {
            var dct = new Dictionary<string, IQueryBuilder<Round>>();

            dct.Add("courseNameFilter", new SimpleFilterBuilder<Round>(new FilterConfig<Round>(x => x.Course.Name), new StringIsNotNullProcessFilterRule()));
            dct.Add("seasonFilter", new SimpleFilterBuilder<Round>(new FilterConfig<Round>(x => x.SeasonId, DynamicUtilitiesEquations.Equal), new NotNullProcessFilterRule()));
            dct.Add("roundDateStartFilter", new SimpleFilterBuilder<Round>(new FilterConfig<Round>(x => x.RoundDate, DynamicUtilitiesEquations.GreaterThanOrEqual), new NotNullProcessFilterRule()));
            dct.Add("roundDateEndFilter", new SimpleFilterBuilder<Round>(new FilterConfig<Round>(x => x.RoundDate, DynamicUtilitiesEquations.LessThanOrEqual), new NotNullProcessFilterRule()));
            dct.Add("handicappedRoundsOnly", new HandicapRoundsOnlyFilter<Round>(new BooleanIsTrueFilterRule()));

            return dct;
        }

        #endregion

    }
}
