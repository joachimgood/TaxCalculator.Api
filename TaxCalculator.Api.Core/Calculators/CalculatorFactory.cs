using TaxCalculator.Api.Data.Entities;

namespace TaxCalculator.Api.Core.Calculators
{
    public static class CalculatorFactory
    {
        public delegate int CalculateFeeDelegate(
            DateTime[] passages,
            int maxDayFee,
            IEnumerable<HourFee> fees,
            IEnumerable<DateOnly> tollFreeDates);

        public static CalculateFeeDelegate GetCityCalculator(string city)
        {
            return city.ToLower() switch
            {
                "gothenburg" => GothenburgCalculator.CalculateFee(),
                _ => throw new Exception("City calcuation not implemented yet"),
            };
        }
    }
}

