namespace TaxCalculator.Api.Data.Interfaces
{
    public interface IFeeRepository
    {
        List<DateTime> GetTollFreeDatesByYear(int year);
        int GetCityMaxDayFee(string city);
        int GetFeeByCityAndHour(string city, DateTime dateTime);
    }
}
