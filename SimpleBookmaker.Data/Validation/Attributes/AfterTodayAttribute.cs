namespace SimpleBookmaker.Data.Validation.Attributes
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class AfterTodayAttribute : ValidationAttribute
    {
        private bool isTodayValid;

        public AfterTodayAttribute(bool isTodayValid = false)
        {
            this.isTodayValid = isTodayValid;
            this.ErrorMessage = "Date must be later than the current day.";
        }

        public override bool IsValid(object value)
        {
            var date = value as DateTime?;

            if(date == null)
            {
                return true;
            }

            if (isTodayValid)
            {
                this.ErrorMessage = "Cannot change kickoff time for games which have passed.";

                return date >= DateTime.UtcNow;
            }

            return date > DateTime.UtcNow;
        }
    }
}
