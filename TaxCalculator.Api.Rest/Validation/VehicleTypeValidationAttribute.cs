#nullable disable

using System.ComponentModel.DataAnnotations;

namespace TaxCalculator.Api.Rest.Validation
{
    public class VehicleTypeValidationAttribute : ValidationAttribute
    {
        private readonly string[] validTypes = { "privatecar", "motorcycle", "tractor", "emergency", "diplomat", "foreigncar", "military", "bus" };
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null)
            {
                string vehicle = (string)value;

                if (validTypes.Contains(vehicle.ToLower()))
                {
                    return ValidationResult.Success;
                }
                else
                {
                    return new ValidationResult($"Vehicle type is not valid.");
                }
            }
            return ValidationResult.Success;
        }
    }
}
