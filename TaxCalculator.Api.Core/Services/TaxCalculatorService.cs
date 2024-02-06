using TaxCalculator.Api.Core.Interfaces;
using TaxCalculator.Api.Data.Entities;
using TaxCalculator.Api.Data.Interfaces;

namespace TaxCalculator.Api.Core.Services
{
    public partial class TaxCalculatorService : ITaxCalculatorService
    {
        private readonly IVehicleRepository _vehicleRepository;
        private readonly IFeeRepository _feeRepository;

        public TaxCalculatorService(IFeeRepository feeRepository, IVehicleRepository vehicleRepository)
        {
            _feeRepository = feeRepository;
            _vehicleRepository = vehicleRepository;
        }

        public int GetTax(string typeOfVehicle, DateTime[] passages, string city)
        {
            Vehicle vehicle = _vehicleRepository.GetVehicleByType(ToVehicleType(typeOfVehicle));

            if (vehicle.IsTollFree) return 0;

            int totalFee = 0;

            foreach (Day day in SplitPassagesIntoDays(passages))
            {
                if (IsTaxFreeDay(day)) continue;

                int dayFee = CalulateDayFee(day.Passages, city);

                totalFee += dayFee;
            }

            return totalFee;
        }

        private static bool IsDayOnWeekend(DateTime dateTime)
        {
            return dateTime.DayOfWeek == DayOfWeek.Sunday || dateTime.DayOfWeek == DayOfWeek.Saturday;
        }

        private static bool IsDayInJuly(DateTime dateTime)
        {
            return dateTime.Month == 7;
        }
        private static List<Day> SplitPassagesIntoDays(DateTime[] passages)
        {
            List<Day> days = new();
            Array.Sort(passages);

            foreach (var passage in passages)
            {
                var matchingDay = days.FirstOrDefault(x => x.Passages.FirstOrDefault().DayOfYear == passage.DayOfYear);
                if (matchingDay == null)
                {
                    days.Add(new Day
                    {
                        Passages = new List<DateTime> { passage }
                    });
                }
                else
                {
                    matchingDay.Passages.Add(passage);
                }
            }

            return days;
        }

        private int CalulateDayFee(List<DateTime> passageOfDay, string city)
        {
            var maxDayFee = _feeRepository.GetCityMaxDayFee(city);
            var passages = passageOfDay.OrderBy(date => date).ToList();
            int dayFee = 0;

            DateTime passageIntervalStart = passages.First();
            int intervalHighestFee = _feeRepository.GetFeeByCityAndHour(city, passageIntervalStart);

            if (passageOfDay.Count == 1)
            {
                return intervalHighestFee; // only one passage, return.
            }

            for (int i = 1; i < passages.Count; i++)
            {
                TimeSpan timeDifference = passages[i] - passageIntervalStart;

                if (timeDifference.TotalMinutes < 60)
                { //within same interval
                    var nextFee = _feeRepository.GetFeeByCityAndHour(city, passages[i]);

                    if (nextFee > intervalHighestFee)
                    {
                        intervalHighestFee = nextFee;
                    }

                }
                else
                {
                    //passage[i] is now new interval start. 
                    dayFee += intervalHighestFee; //add  highest feed to dayfee.

                    // set passage[i] to interval start, and fee to highest interval fee.
                    passageIntervalStart = passages[i];
                    intervalHighestFee = _feeRepository.GetFeeByCityAndHour(city, passages[i]);
                }
                if (i == passages.Count - 1)
                {
                    dayFee += intervalHighestFee;
                }
            }

            return Math.Min(dayFee, maxDayFee);
        }

        private static VehicleType ToVehicleType(string vehicle)
        {
            if (Enum.TryParse(vehicle, true, out VehicleType result))
            {
                return result;
            }
            else
            {
                throw new ArgumentException("Invalid vechicle type");
            }
        }

        private bool IsTaxFreeDay(Day day)
        {

            List<DateTime> tollFreeDates = new();
            var year = day.Passages.First().Year;
            if (!tollFreeDates.Any() || year != tollFreeDates.FirstOrDefault().Year)
            {
                tollFreeDates = _feeRepository.GetTollFreeDatesByYear(year);
            }

            if (tollFreeDates.Any(x => x.DayOfYear == day.Passages.First().DayOfYear))
            {
                return true;
            }

            if (IsDayOnWeekend(day.Passages.First()) || IsDayInJuly(day.Passages.First()))
            {
                return true;
            }

            return false;
        }
    }
}