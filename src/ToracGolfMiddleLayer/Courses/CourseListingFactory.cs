using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToracGolf.MiddleLayer.Courses;
using ToracGolf.MiddleLayer.EFModel.Tables;
using ToracGolf.MiddleLayer.GridCommon.Filters;
using ToracGolf.MiddleLayer.GridCommon.Filters.QueryBuilder;
using ToracGolf.MiddleLayer.ListingFactories;
using static ToracLibrary.Core.ExpressionTrees.API.ExpressionBuilder;

namespace ToracGolf.MiddleLayer.Rounds
{
    public class CourseListingFactory : IListingFactory<Course, CourseListingData>
    {

        #region Constructor

        public CourseListingFactory(IDictionary<string, Func<IQueryable<Course>, ListingFactoryParameters, IOrderedQueryable<Course>>> sortByConfiguration,
                                    IDictionary<string, IQueryBuilder<Course>> filterConfiguration)
        {
            SortByConfiguration = sortByConfiguration;
            FilterConfiguration = filterConfiguration;
        }

        #endregion

        #region Properties

        public IDictionary<string, Func<IQueryable<Course>, ListingFactoryParameters, IOrderedQueryable<Course>>> SortByConfiguration { get; }

        public IDictionary<string, IQueryBuilder<Course>> FilterConfiguration { get; }

        #endregion

        #region Lookup

        public static IDictionary<string, Func<IQueryable<Course>, ListingFactoryParameters, IOrderedQueryable<Course>>> SortByConfigurationBuilder()
        {
            var dct = new Dictionary<string, Func<IQueryable<Course>, ListingFactoryParameters, IOrderedQueryable<Course>>>();

            dct.Add(CourseListingSortOrder.CourseListingSortEnum.CourseNameAscending.ToString(), (x, param) => x.OrderBy(y => y.Name));
            dct.Add(CourseListingSortOrder.CourseListingSortEnum.CourseNameDescending.ToString(), (x, param) => x.OrderByDescending(y => y.Name));

            dct.Add(CourseListingSortOrder.CourseListingSortEnum.EasiestCourses.ToString(), (x, param) => x.OrderBy(y => y.CourseTeeLocations.Max(z => z.Slope)));
            dct.Add(CourseListingSortOrder.CourseListingSortEnum.HardestCourses.ToString(), (x, param) => x.OrderByDescending(y => y.CourseTeeLocations.Max(z => z.Slope)));

            dct.Add(CourseListingSortOrder.CourseListingSortEnum.MostTimesPlayed.ToString(), (x, param) => x.OrderByDescending(z => param.DbContext.Rounds.Count(y => y.CourseId == z.CourseId && y.UserId == param.UserId)));

            return dct;
        }

        public static IDictionary<string, IQueryBuilder<Course>> FilterByConfigurationBuilder()
        {
            var dct = new Dictionary<string, IQueryBuilder<Course>>();

            dct.Add("courseNameFilter", new SimpleFilterBuilder<Course>(new FilterConfig<Course>(x => x.Name)));
            dct.Add("stateFilter", new SimpleFilterBuilder<Course>(new FilterConfig<Course>(x => x.StateId, DynamicUtilitiesEquations.Equal)));

            return dct;
        }

        #endregion

    }
}
