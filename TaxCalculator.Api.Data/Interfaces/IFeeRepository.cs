using TaxCalculator.Api.Data.Entities;

namespace TaxCalculator.Api.Data.Interfaces
{
    public interface IFeeRepository
    {
        List<DateOnly> GetTollFreeDates();
        int GetCityMaxDayFee(string city);
        int GetFeeByCityAndHour(string city, DateTime dateTime);
        IEnumerable<HourFee> GetFeesByCity(string city);
    }
}
