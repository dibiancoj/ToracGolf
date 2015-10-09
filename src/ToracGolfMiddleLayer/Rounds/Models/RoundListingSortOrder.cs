using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using ToracGolf.MiddleLayer.GridCommon;
using ToracLibrary.AspNet.EnumUtilities;

namespace ToracGolf.MiddleLayer.Rounds.Models
{

    public class RoundListingSortOrder
    {

        #region Enum

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

            [Description("Best Score")]
            BestScores = 4,

            [Description("Worse Score")]
            WorseScores= 5,

        }

        #endregion

        #region Course Listing Sort Drop Down Builder

        public static IList<SortOrderViewModel> BuildDropDownValues()
        {
            var lst = new List<SortOrderViewModel>();

            foreach (var enumValue in EnumUtility.GetValuesLazy<RoundListingSortEnum>())
            {
                lst.Add(new SortOrderViewModel(Convert.ToInt32(enumValue),
                        enumValue.ToString(),
                        EnumUtility.CustomAttributeGet<DescriptionAttribute>(enumValue).Description));
            }

            return lst;
        }

        #endregion

    }

}
