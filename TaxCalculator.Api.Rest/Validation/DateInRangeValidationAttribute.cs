#nullable disable

using System.ComponentModel.DataAnnotations;

namespace TaxCalculator.Api.Rest.Validation
{
    public class DateInRangeValidationAttribute : ValidationAttribute
    {
        private readonly DateTime _minDate;
        private readonly DateTime _maxDate;

        public DateInRangeValidationAttribute(string minDate, string maxDate)
        {
            _minDate = DateTime.ParseExact(minDate, "yyyy-MM-dd", null);
            _maxDate = DateTime.ParseExact(maxDate, "yyyy-MM-dd", null);
        }
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null && value is DateTime[] dates)
            {
                foreach (var date in dates)
                {
                    if (date < _minDate || date > _maxDate)
                    {
                        return new ValidationResult($"The passages must be between {_minDate.ToShortDateString()} and {_maxDate.ToShortDateString()}.");
                    }
                }
                return ValidationResult.Success;
            }
            return ValidationResult.Success; 
        }
    }

}
