namespace TaxCalculator.Api.Data.Entities
{
    public class CityFees
    {
        public string CityName { get; set; }
        public int MaxDayFeeOfCity { get; set; }
        public List<HourFee> HourFees { get; set; }
    }
}
