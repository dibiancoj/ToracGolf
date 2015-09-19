using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToracGolf.MiddleLayer.CustomValidations
{

    /// <summary>
    /// In asp.net when you have a list, you want to ensure you have x amount of items at the minimum in your list.
    /// </summary>
    public class EnsureMinimumElementsAttribute : System.ComponentModel.DataAnnotations.ValidationAttribute
    {

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="MinimumNumberOfElementsAllowedToSet">The minimum number of elements. If false, validation will fail</param>
        public EnsureMinimumElementsAttribute(int MinimumNumberOfElementsAllowedToSet)
        {
            MinimumNumberOfElementsAllowed = MinimumNumberOfElementsAllowedToSet;
        }

        #endregion

        #region Properties

        /// <summary>
        /// The minimum number of elements. If false, validation will fail
        /// </summary>
        public int MinimumNumberOfElementsAllowed { get; }

        #endregion

        #region Override Methods

        /// <summary>
        /// Override method to test if this object is valid and passes validation
        /// </summary>
        /// <param name="value">value to check</param>
        /// <returns>Does it pass validation? Is the model valid</returns>
        public override bool IsValid(object value)
        {
            //is the value null and we want more then 0 elements, fail it
            if (value == null && MinimumNumberOfElementsAllowed > 0)
            {
                return false;
            }

            //try to case this to an ilist so we can grab the count
            var CastedToIListTry = value as IList;

            //were we able to cast this?
            if (CastedToIListTry != null)
            {
                //we have it in a list, just return the check
                return IsValidHelperMethod(CastedToIListTry.Count, MinimumNumberOfElementsAllowed);
            }

            //let's try to cast this to ienumerable
            var CastedToIEnumerableTry = value as IEnumerable;

            //could we cast this?
            if (CastedToIEnumerableTry == null)
            {
                //throw an exception
                throw new InvalidCastException("Not Able To Cast Property To IEnumerable In EnsureMinimumElementsAttribute. Type Is = " + value.GetType().Name);
            }

            //grab the enumerator
            IEnumerator CollectionEnumerator = CastedToIEnumerableTry.GetEnumerator();

            //holds the tally of how many items we have
            int CountOfItems = 0;

            //keep looping until we reach the end
            while (CollectionEnumerator.MoveNext())
            {
                //increase the tally
                CountOfItems++;
            }

            //let's try to count these item and compare it
            return IsValidHelperMethod(CountOfItems, MinimumNumberOfElementsAllowed);
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Common helper method to compare the same logic in all scenarios
        /// </summary>
        /// <param name="CountInCollection">Number of items in the collection found</param>
        /// <param name="MinimumNumberOfElementsAllowedToValidate">The minimum number of elements. If false, validation will fail</param>
        /// <returns>Does it pass validation? Is the model valid</returns>
        private static bool IsValidHelperMethod(int CountInCollection, int MinimumNumberOfElementsAllowedToValidate)
        {
            //return the result
            return CountInCollection >= MinimumNumberOfElementsAllowedToValidate;
        }

        #endregion

    }

}
