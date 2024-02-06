using TaxCalculator.Api.Data.Entities;
using TaxCalculator.Api.Data.Interfaces;

namespace TaxCalculator.Api.Data.Repositories
{
    public class FeeRepository : IFeeRepository
    {
        private readonly List<DateTime> _tollFreeDates;
        private readonly List<CityFees> _cityFees;
        public FeeRepository()
        {
            _tollFreeDates = new List<DateTime>
            {
                new DateTime(2013, 1, 1),
                new DateTime(2013, 3, 28),
                new DateTime(2013, 3, 29),
                new DateTime(2013, 4, 1),
                new DateTime(2013, 4, 30),
                new DateTime(2013, 5, 1),
                new DateTime(2013, 5, 8),
                new DateTime(2013, 5, 9),
                new DateTime(2013, 6, 5),
                new DateTime(2013, 6, 6),
                new DateTime(2013, 6, 21),
                new DateTime(2013, 11, 1),
                new DateTime(2013, 12, 24),
                new DateTime(2013, 12, 25),
                new DateTime(2013, 12, 26),
                new DateTime(2013, 12, 31)

            };

            _cityFees = new List<CityFees>
            {
                new CityFees
                {
                    CityName = "gothenburg",
                    MaxDayFeeOfCity = 60,
                    HourFees = new List<HourFee>
                    {
                        new HourFee{StartTime = TimeSpan.Parse("06:00:00"),  EndTime= TimeSpan.Parse("06:30:00"), Fee = 8},
                        new HourFee{StartTime = TimeSpan.Parse("06:30:00"),  EndTime= TimeSpan.Parse("07:00:00"), Fee = 13},
                        new HourFee{StartTime = TimeSpan.Parse("07:00:00"),  EndTime= TimeSpan.Parse("08:00:00"), Fee = 18},
                        new HourFee{StartTime = TimeSpan.Parse("08:00:00"),  EndTime= TimeSpan.Parse("08:30:00"), Fee = 13},
                        new HourFee{StartTime = TimeSpan.Parse("08:30:00"),  EndTime= TimeSpan.Parse("15:00:00"), Fee = 8},
                        new HourFee{StartTime = TimeSpan.Parse("15:00:00"),  EndTime= TimeSpan.Parse("15:30:00"), Fee = 13},
                        new HourFee{StartTime = TimeSpan.Parse("15:30:00"),  EndTime= TimeSpan.Parse("17:00:00"), Fee = 18},
                        new HourFee{StartTime = TimeSpan.Parse("17:00:00"),  EndTime= TimeSpan.Parse("18:00:00"), Fee = 13},
                        new HourFee{StartTime = TimeSpan.Parse("18:00:00"),  EndTime= TimeSpan.Parse("18:30:00"), Fee = 8},
                        new HourFee{StartTime = TimeSpan.Parse("18:30:00"),  EndTime= TimeSpan.Parse("06:00:00"), Fee = 0},
                    }
                }
            };

       
            _tollFreeDates.AddRange(Enumerable.Range(1, 30).Select(day => new DateTime(2013, 7, day)));
        }

        public List<DateTime> GetTollFreeDatesByYear(int year)
        {
            return _tollFreeDates.Where(x => x.Year == year).ToList();
        }

        public int GetCityMaxDayFee(string city)
        {
            var matchingCity = _cityFees.FirstOrDefault(x => string.Equals(x.CityName,city, StringComparison.OrdinalIgnoreCase));
            return matchingCity?.MaxDayFeeOfCity ?? 0;
        }

        public int GetFeeByCityAndHour(string city, DateTime dateTime)
        {

            var matchingCity = _cityFees.FirstOrDefault(x => string.Equals(x.CityName, city, StringComparison.OrdinalIgnoreCase));
            return matchingCity?.HourFees?.FirstOrDefault(hourFee => dateTime.TimeOfDay >= hourFee.StartTime && dateTime.TimeOfDay < hourFee.EndTime)?.Fee ?? 0;
        }
    }

}
