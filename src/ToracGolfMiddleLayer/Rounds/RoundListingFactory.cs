using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToracGolf.MiddleLayer.ListingFactories;
using ToracGolf.MiddleLayer.Rounds.Models;

namespace ToracGolf.MiddleLayer.Rounds
{
    public class RoundListingFactory : IListingFactory<RoundListingData>
    {

        #region Constructor

        public RoundListingFactory(IDictionary<string, Func<IQueryable<RoundListingData>, ListingFactoryParameters, IOrderedQueryable<RoundListingData>>> sortByConfiguration)
        {
            SortByConfiguration = sortByConfiguration;
        }

        #endregion

        #region Properties

        public IDictionary<string, Func<IQueryable<RoundListingData>, ListingFactoryParameters, IOrderedQueryable<RoundListingData>>> SortByConfiguration { get; }

        #endregion

        #region Lookup

        public static IDictionary<string, Func<IQueryable<RoundListingData>, ListingFactoryParameters, IOrderedQueryable<RoundListingData>>> SortByConfigurationBuilder()
        {
            var dct = new Dictionary<string, Func<IQueryable<RoundListingData>, ListingFactoryParameters, IOrderedQueryable<RoundListingData>>>();

            dct.Add(RoundListingSortOrder.RoundListingSortEnum.CourseNameAscending.ToString(), (x, param) => x.OrderBy(y => y.CourseName));
            dct.Add(RoundListingSortOrder.RoundListingSortEnum.CourseNameDescending.ToString(), (x, param) => x.OrderByDescending(y => y.CourseName));

            dct.Add(RoundListingSortOrder.RoundListingSortEnum.RoundDateAscending.ToString(), (x, param) => x.OrderBy(y => y.RoundDate));
            dct.Add(RoundListingSortOrder.RoundListingSortEnum.RoundDateDescending.ToString(), (x, param) => x.OrderByDescending(y => y.RoundDate));

            dct.Add(RoundListingSortOrder.RoundListingSortEnum.BestRawScore.ToString(), (x, param) => x.OrderBy(y => y.Score));
            dct.Add(RoundListingSortOrder.RoundListingSortEnum.WorseRawScore.ToString(), (x, param) => x.OrderByDescending(y => y.Score));

            dct.Add(RoundListingSortOrder.RoundListingSortEnum.RoundHandicapAscending.ToString(), (x, param) => x.OrderBy(y => y.RoundHandicap));
            dct.Add(RoundListingSortOrder.RoundListingSortEnum.RoundHandicapDescending.ToString(), (x, param) => x.OrderByDescending(y => y.RoundHandicap));

            return dct;
        }

        #endregion

    }
}
