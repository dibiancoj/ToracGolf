using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToracGolf.MiddleLayer.Courses;
using ToracGolf.MiddleLayer.EFModel.Tables;
using ToracGolf.MiddleLayer.ListingFactories;

namespace ToracGolf.MiddleLayer.Rounds
{
    public class CourseListingFactory : IListingFactory<Course>
    {

        #region Constructor

        public CourseListingFactory(IDictionary<string, Func<IQueryable<Course>, ListingFactoryParameters, IOrderedQueryable<Course>>> sortByConfiguration)
        {
            SortByConfiguration = sortByConfiguration;
        }

        #endregion

        #region Properties

        public IDictionary<string, Func<IQueryable<Course>, ListingFactoryParameters, IOrderedQueryable<Course>>> SortByConfiguration { get; }

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

        #endregion

    }
}
