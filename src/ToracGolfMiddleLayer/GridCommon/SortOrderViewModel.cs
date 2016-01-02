using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using ToracLibrary.Core.EnumUtilities;

namespace ToracGolf.MiddleLayer.GridCommon
{
    public class SortOrderViewModel
    {

        #region Constructor

        public SortOrderViewModel(int sortOrder, string value, string description)
        {
            SortOrder = sortOrder;
            Value = value;
            Description = description;
        }

        #endregion

        #region Properties

        public int SortOrder { get; }
        public string Value { get; }
        public string Description { get; }

        #endregion

        #region Static Methods

        public static IEnumerable<SortOrderViewModel> BuildListFromEnumLazy<TEnum>()
            where TEnum : struct, IConvertible
        {
            foreach (var enumValue in EnumUtility.GetValuesLazy<TEnum>())
            {
                yield return (new SortOrderViewModel(Convert.ToInt32(enumValue),
                              enumValue.ToString(),
                              EnumUtility.CustomAttributeGet<DescriptionAttribute>(enumValue as Enum).Description));
            }
        }

        #endregion

    }
}
