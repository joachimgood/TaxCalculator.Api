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

            foreach (var day in SplitPassagesIntoDays(passages))
            {
                
                if (IsTaxFreeDay(day.Key)) continue;

                int dayFee = CalulateDayFee(day.Value.ToList(), city);

                totalFee += dayFee;
            }

            return totalFee;
        }

        private static bool IsDayOnWeekend(DateOnly date)
        {
            return date.DayOfWeek == DayOfWeek.Sunday || date.DayOfWeek == DayOfWeek.Saturday;
        }

        private static bool IsDayInJuly(DateOnly date)
        {
            return date.Month == 7;
        }
        private static Dictionary<DateOnly, IEnumerable<DateTime>> SplitPassagesIntoDays(DateTime[] passages)
        {
            return passages
                .Select(passage => passage.Date)
                .Distinct()
                .ToDictionary(DateOnly.FromDateTime, date => passages.Where(passage => passage.Date == date));
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

        private bool IsTaxFreeDay(DateOnly day)
        {

            List<DateTime> tollFreeDates = new();
            if (!tollFreeDates.Any() || day.Year != tollFreeDates.FirstOrDefault().Year)
            {
                tollFreeDates = _feeRepository.GetTollFreeDatesByYear(day.Year);
            }

            if (tollFreeDates.Any(x => x.DayOfYear == day.DayOfYear))
            {
                return true;
            }

            if (IsDayOnWeekend(day) || IsDayInJuly(day))
            {
                return true;
            }

            return false;
        }
    }
}