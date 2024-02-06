using TaxCalculator.Api.Rest.Controllers;

namespace TaxCalculator.Api.Rest.Validation
{
    public static class RequestValidator
    {
        internal static bool ValidateCalculateTaxRequest(CalculateTaxRequest request)
        {
            if (!IsValidVehicleType(request.Vehicle)) return false;

            if (request.Passages.Any(x => x.Year != 2013)) return false;

            if (!string.Equals(request.City, "gothenburg", StringComparison.OrdinalIgnoreCase)) return false;

            return true;
        }

        private static bool IsValidVehicleType(string vehicle)
        {
            return vehicle.ToLower() switch
            {
                "privatecar" => true,
                "motorcycle" => true,
                "tractor" => true,
                "emergency" => true,
                "diplomat" => true,
                "foreigncar" => true,
                "military" => true,
                "bus" => true,
                _ => false,
            };
        }
    }
}
