using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ToracGolf.MiddleLayer.CustomValidators
{

    public class FairwaysHitRequiredAttribute : ValidationAttribute
    {

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public FairwaysHitRequiredAttribute(string dependentPropertyName)
        {
            DependentPropertyName = dependentPropertyName;
        }

        #endregion

        #region Properties

        /// <summary>
        /// The minimum number of elements. If false, validation will fail
        /// </summary>
        public string DependentPropertyName { get; }

        #endregion

        #region Override Methods

        /// <summary>
        /// Override method to test if this object is valid and passes validation
        /// </summary>
        /// <param name="value">value to check</param>
        /// <returns>Does it pass validation? Is the model valid</returns>
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            //is this value null or not
            var propertyIsNull = value == null;

            //is the depend. property null
            var depPropertyIsNull = validationContext.ObjectType.GetProperty(DependentPropertyName).GetValue(validationContext.ObjectInstance) == null;

            //return the result
            if (propertyIsNull == depPropertyIsNull)
            {
                return ValidationResult.Success;
            }

            return new ValidationResult(ErrorMessage);
        }

        #endregion

    }

}
