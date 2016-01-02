using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using ToracGolf.MiddleLayer.Courses;
using ToracGolf.MiddleLayer.EFModel.Tables;
using ToracGolf.MiddleLayer.GridCommon.Filters;
using ToracGolf.MiddleLayer.GridCommon.Filters.ProcessFilterRules;
using ToracGolf.MiddleLayer.GridCommon.Filters.QueryBuilder;
using ToracGolf.MiddleLayer.ListingFactories;
using static ToracLibrary.Core.ExpressionTrees.API.ExpressionBuilder;

namespace ToracGolf.MiddleLayer.Courses
{
    public class CourseListingFactory : IListingFactory<CourseListingFactory.CourseListingSortEnum, Course, CourseListingData>
    {

        #region Constructor

        public CourseListingFactory(IDictionary<CourseListingSortEnum, Func<IQueryable<Course>, ListingFactoryParameters, IOrderedQueryable<Course>>> sortByConfiguration,
                                    IDictionary<string, IQueryBuilder<Course>> filterConfiguration)
        {
            SortByConfiguration = sortByConfiguration;
            FilterConfiguration = filterConfiguration;
        }

        #endregion

        #region Properties

        public IDictionary<CourseListingSortEnum, Func<IQueryable<Course>, ListingFactoryParameters, IOrderedQueryable<Course>>> SortByConfiguration { get; }

        public IDictionary<string, IQueryBuilder<Course>> FilterConfiguration { get; }

        #endregion

        #region Sort Enum

        public enum CourseListingSortEnum
        {

            [Description("Most Times Played")]
            MostTimesPlayed = 0,

            [Description("Course Name Ascending")]
            CourseNameAscending = 1,

            [Description("Course Name Descending")]
            CourseNameDescending = 2,

            [Description("Hardest Courses")]
            HardestCourses = 3,

            [Description("Easiest Courses")]
            EasiestCourses = 4
        }

        #endregion

        #region Lookup

        public static IDictionary<CourseListingSortEnum, Func<IQueryable<Course>, ListingFactoryParameters, IOrderedQueryable<Course>>> SortByConfigurationBuilder()
        {
            var dct = new Dictionary<CourseListingSortEnum, Func<IQueryable<Course>, ListingFactoryParameters, IOrderedQueryable<Course>>>();

            dct.Add(CourseListingSortEnum.CourseNameAscending, (x, param) => x.OrderBy(y => y.Name));
            dct.Add(CourseListingSortEnum.CourseNameDescending, (x, param) => x.OrderByDescending(y => y.Name));

            dct.Add(CourseListingSortEnum.EasiestCourses, (x, param) => x.OrderBy(y => y.CourseTeeLocations.Max(z => z.Slope)));
            dct.Add(CourseListingSortEnum.HardestCourses, (x, param) => x.OrderByDescending(y => y.CourseTeeLocations.Max(z => z.Slope)));

            dct.Add(CourseListingSortEnum.MostTimesPlayed, (x, param) => x.OrderByDescending(z => param.DbContext.Rounds.Count(y => y.CourseId == z.CourseId && y.UserId == param.UserId)));

            return dct;
        }

        public static IDictionary<string, IQueryBuilder<Course>> FilterByConfigurationBuilder()
        {
            var dct = new Dictionary<string, IQueryBuilder<Course>>();

            dct.Add("courseNameFilter", new SimpleFilterBuilder<Course>(new FilterConfig<Course>(x => x.Name), new StringIsNotNullProcessFilterRule()));
            dct.Add("stateFilter", new SimpleFilterBuilder<Course>(new FilterConfig<Course>(x => x.StateId, DynamicUtilitiesEquations.Equal), new NotNullProcessFilterRule()));

            return dct;
        }

        #endregion

    }
}
