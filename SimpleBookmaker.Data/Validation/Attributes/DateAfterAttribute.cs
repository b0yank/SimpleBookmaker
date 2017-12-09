namespace SimpleBookmaker.Data.Validation.Attributes
{
    using System;
    using System.ComponentModel.DataAnnotations;

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class DateAfterAttribute : ValidationAttribute
    {
        private const string DefaultErrorMessage = "{0} must be after {1}.";
        private readonly string otherPropertyName; 

        public DateAfterAttribute(string otherPropertyName)
        {
            this.otherPropertyName = otherPropertyName;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null)
            {
                var otherProperty = validationContext.ObjectInstance.GetType()
                                   .GetProperty(this.otherPropertyName);

                var otherPropertyValue = otherProperty
                              .GetValue(validationContext.ObjectInstance, null);

                var thisPropertyValue = value as DateTime?;

                if (thisPropertyValue == null)
                {
                    return ValidationResult.Success;
                }

                if (thisPropertyValue <= (DateTime?)otherPropertyValue)
                {
                    return new ValidationResult(
                      FormatErrorMessage(validationContext.DisplayName));
                }
            }

            return ValidationResult.Success;
        }
    }
}
