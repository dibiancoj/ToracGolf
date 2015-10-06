using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using ToracLibrary.AspNet.EnumUtilities;

namespace ToracGolf.MiddleLayer.Courses
{

    public class CourseListingSortOrder
    {

        #region Enum

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

        #region Course Listing Sort Drop Down Builder

        public static IList<CourseListingSortOrderModel> BuildDropDownValues()
        {
            var lst = new List<CourseListingSortOrderModel>();

            foreach (var enumValue in EnumUtility.GetValuesLazy<CourseListingSortEnum>())
            {
                lst.Add(new CourseListingSortOrderModel(Convert.ToInt32(enumValue),
                        enumValue.ToString(),
                        EnumUtility.CustomAttributeGet<DescriptionAttribute>(enumValue).Description));
            }

            return lst;
        }

        #endregion


    }

    public class CourseListingSortOrderModel
    {

        public CourseListingSortOrderModel(int sortOrder, string value, string description)
        {
            SortOrder = sortOrder;
            Value = value;
            Description = description;
        }

        public int SortOrder { get; }
        public string Value { get; }
        public string Description { get; }
    }

}
