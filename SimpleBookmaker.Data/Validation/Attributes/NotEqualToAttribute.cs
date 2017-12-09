namespace SimpleBookmaker.Data.Validation.Attributes
{
    using System;
    using System.ComponentModel.DataAnnotations;

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true, Inherited = true)]
    public class NotEqualToAttribute : ValidationAttribute
    {
        private const string DefaultErrorMessage = "{0} cannot be the same as {1}.";

        private string otherPropertyName;
        private string otherPropertyDisplayName;
        

        public NotEqualToAttribute(string otherPropertyName)
          : base(DefaultErrorMessage)
        {
            if (string.IsNullOrEmpty(otherPropertyName))
            {
                throw new ArgumentNullException("otherProperty");
            }

            this.otherPropertyName = otherPropertyName;
            otherPropertyDisplayName = otherPropertyName;
        }

        public override string FormatErrorMessage(string name)
        {
            return string.Format(ErrorMessageString, name, otherPropertyDisplayName);
        }

        protected override ValidationResult IsValid(object value,
                              ValidationContext validationContext)
        {
            if (value != null)
            {
                var otherProperty = validationContext.ObjectInstance.GetType()
                                   .GetProperty(this.otherPropertyName);


                var otherPropertyValue = otherProperty
                              .GetValue(validationContext.ObjectInstance, null);

                if (value.Equals(otherPropertyValue))
                {
                    var otherPropertyAttributes = otherProperty.GetCustomAttributes(false);

                    foreach (var attribute in otherPropertyAttributes)
                    {
                        var displayAttribute = attribute as DisplayAttribute;

                        if (displayAttribute != null)
                        {
                            this.otherPropertyDisplayName = displayAttribute.Name;
                        }
                    }

                    return new ValidationResult(
                      FormatErrorMessage(validationContext.DisplayName));
                }
            }

            return ValidationResult.Success;
        }
    }
}
