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
                int dayFee = CalulateDayFee(day, city);
                totalFee += dayFee;
            }

            return totalFee;
        }

      
        private static Dictionary<DateOnly, IEnumerable<DateTime>> SplitPassagesIntoDays(DateTime[] passages)
        {
            return passages
                .Select(passage => passage.Date)
                .Distinct()
                .ToDictionary(DateOnly.FromDateTime, date => passages.Where(passage => passage.Date == date));
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

        //extract
        private int CalulateDayFee(KeyValuePair<DateOnly, IEnumerable<DateTime>> day, string city)
        {
            if (IsTaxFreeDay(day.Key)) return 0;

            var maxDayFee = _feeRepository.GetCityMaxDayFee(city);
            var passages = day.Value.OrderBy(date => date).ToList();
            int dayFee = 0;

            DateTime passageIntervalStart = passages.First();
            int intervalHighestFee = _feeRepository.GetFeeByCityAndHour(city, passageIntervalStart);

            if (passages.Count == 1)
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

        //Todo: Extract
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

        private static bool IsDayOnWeekend(DateOnly date)
        {
            return date.DayOfWeek == DayOfWeek.Sunday || date.DayOfWeek == DayOfWeek.Saturday;
        }

        private static bool IsDayInJuly(DateOnly date)
        {
            return date.Month == 7;
        }
    }
}