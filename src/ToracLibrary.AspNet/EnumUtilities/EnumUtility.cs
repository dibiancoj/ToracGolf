using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace ToracLibrary.AspNet.EnumUtilities
{

    /// <summary>
    /// Utilities for enum's
    /// </summary>
    public static class EnumUtility
    {

        #region Get Values

        /// <summary>
        /// Retrieve all the members of an enum
        /// </summary>
        /// <typeparam name="T">Type Of The Enum</typeparam>
        /// <returns>IEnumerable of your enum's</returns>
        /// <remarks>Usage = var values = EnumUtil.GetValues &lt;Foos&lt;();</remarks>
        public static IEnumerable<T> GetValuesLazy<T>() where T : struct
        {
            //loop through the types
            foreach (var EnumType in Enum.GetValues(typeof(T)))
            {
                //return this value
                yield return (T)EnumType;
            }
        }

        #endregion

        #region Get Custom Attribute Off Of Enum Member

        /// <summary>
        /// Method will retrieve a customm attribute off of the enum passed in.
        /// the items in your enum.
        /// </summary>
        /// <typeparam name="T">Custom Attribute Type To Look For</typeparam>
        /// <param name="EnumValueToRetrieve">Enum Value To Retrieve The Attribute Off Of</param>
        /// <returns>Description Attribute Value Or Null If The Description Tag Is Not Found</returns>
        /// <remarks>Custom Attribute If Found. Otherwise Null</remarks>
        public static T CustomAttributeGet<T>(Enum EnumValueToRetrieve) where T : Attribute
        {
            //System.ComponentModel.DescriptionAttribute
            //[TestAttribute("Equals This Number")]
            //Equals = 1

            //grab the type of the enum passed in, then grab the field info object from the enum value passed in
            FieldInfo thisFieldInfo = EnumValueToRetrieve.GetType().GetField(EnumValueToRetrieve.ToString());

            //grab the custom attributes now 
            return thisFieldInfo.GetCustomAttribute(typeof(T)) as T;
        }

        #endregion

    }

}
