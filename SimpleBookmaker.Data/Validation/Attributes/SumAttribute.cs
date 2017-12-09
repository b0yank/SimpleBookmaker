namespace SimpleBookmaker.Data.Validation.Attributes
{
    using System.ComponentModel.DataAnnotations;

    public class SumAttribute : ValidationAttribute
    {
        private readonly int sum;
        private readonly string otherPropertyName; 

        public SumAttribute(int sum, string otherPropertyName)
        {
            this.sum = sum;
            this.otherPropertyName = otherPropertyName;
        }

        protected override ValidationResult IsValid(object value,
                              ValidationContext validationContext)
        {
            if (value != null)
            {
                var otherProperty = validationContext.ObjectInstance.GetType()
                                   .GetProperty(this.otherPropertyName);


                var otherPropertyValue = (int)otherProperty
                              .GetValue(validationContext.ObjectInstance, null);

                var propertyValue = value as int?;

                if (propertyValue != null)
                {
                    if (propertyValue + otherPropertyValue != sum)
                    {
                        return new ValidationResult(
                            FormatErrorMessage(validationContext.DisplayName));
                    }
                }
            }

            return ValidationResult.Success;
        }
    }
}
