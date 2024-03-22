using TaxCalculator.Api.Data.Entities;
using TaxCalculator.Api.Data.Interfaces;

namespace TaxCalculator.Api.Data.Repositories
{
    public class FeeRepository : IFeeRepository
    {
        private readonly List<DateOnly> _tollFreeDates;
        private readonly List<CityFees> _cityFees;
        public FeeRepository()
        {
            _tollFreeDates = new List<DateOnly>
            {
                new DateOnly(2013, 1, 1),
                new DateOnly(2013, 3, 28),
                new DateOnly(2013, 3, 29),
                new DateOnly(2013, 4, 1),
                new DateOnly(2013, 4, 30),
                new DateOnly(2013, 5, 1),
                new DateOnly(2013, 5, 8),
                new DateOnly(2013, 5, 9),
                new DateOnly(2013, 6, 5),
                new DateOnly(2013, 6, 6),
                new DateOnly(2013, 6, 21),
                new DateOnly(2013, 11, 1),
                new DateOnly(2013, 12, 24),
                new DateOnly(2013, 12, 25),
                new DateOnly(2013, 12, 26),
                new DateOnly(2013, 12, 31)

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

       
            _tollFreeDates.AddRange(Enumerable.Range(1, 30).Select(day => new DateOnly(2013, 7, day)));
        }

        public List<DateOnly> GetTollFreeDates()
        {
            return _tollFreeDates;
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
        public IEnumerable<HourFee> GetFeesByCity(string city)
        {
            var matchingCity = _cityFees.SingleOrDefault(x => string.Equals(x.CityName, city, StringComparison.OrdinalIgnoreCase));
            if (matchingCity == null || matchingCity?.HourFees == null) throw new Exception("City fees not found");
            return matchingCity.HourFees;
        }
    }

}
