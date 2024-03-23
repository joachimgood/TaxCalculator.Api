#nullable disable

using System.ComponentModel.DataAnnotations;

namespace TaxCalculator.Api.Rest.Validation
{
    public class CityValidationAttribute : ValidationAttribute
    {
        private readonly string[] validCities = { "gothenburg" };
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null)
            {
                string city = (string)value;

                if (validCities.Contains(city.ToLower()))
                {
                    return ValidationResult.Success;
                }
                else
                {
                    return new ValidationResult($"City is not valid.");
                }
            }
            return new ValidationResult($"City field is required");
        }
    }

}
