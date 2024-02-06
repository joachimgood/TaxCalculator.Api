namespace TaxCalculator.Api.Data.Entities
{
    public class HourFee
    {
        public int Fee { get; init; }
        public TimeSpan StartTime { get; init; }
        public TimeSpan EndTime { get; init; }
    }
}
