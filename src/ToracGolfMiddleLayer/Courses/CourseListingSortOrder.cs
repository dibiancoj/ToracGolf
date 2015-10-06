using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using ToracLibrary.AspNet.EnumUtilities;

namespace ToracGolf.MiddleLayer.Courses
{

    public static class CourseListingSortOrder
    {

        public enum CourseListingSortEnum
        {

            [Description("Most Times Played")]
            MostTimesPlayed = 0,

            [Description("Hardest Courses")]
            HardestCourses = 1,

            [Description("Easiest Courses")]
            EasiestCourses = 2
        }

        #region Course Listing Sort Drop Down Builder

        public static IDictionary<string, string> BuildDropDownValues()
        {
            var dct = new Dictionary<string, string>();

            foreach (var enumValue in EnumUtility.GetValuesLazy<CourseListingSortEnum>())
            {
                dct.Add(enumValue.ToString(), EnumUtility.CustomAttributeGet<DescriptionAttribute>(enumValue).Description);
            }

            return dct;
        }

        #endregion


    }

}
