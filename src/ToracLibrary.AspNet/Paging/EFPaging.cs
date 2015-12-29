using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ToracLibrary.AspNet.Paging
{
    public static class EFPaging
    {

        /// <param name="pageId">0 base index that holds what page we are on</param>
        public static IQueryable<T> PageEfQuery<T>(IQueryable<T> queryToPage, int pageId, int recordsPerPage)
            where T : class
        {
            //how many items to skip
            int skipAmount = pageId * recordsPerPage;

            //go set the skip amount and return the query
            return queryToPage.Skip(skipAmount).Take(recordsPerPage);
        }

    }
}
