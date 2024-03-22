using TaxCalculator.Api.Core.Calculators;
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

        public int GetTax(string vehicleType, DateTime[] passages, string city)
        {
            if (passages.Length == 0) 
                return 0;

            var vehicle = _vehicleRepository.GetVehicleByType(ToVehicleType(vehicleType));
            if (vehicle.IsTollFree) 
                return 0;

            var maxDayFee = _feeRepository.GetCityMaxDayFee(city);
            var fees = _feeRepository.GetFeesByCity(city);
            var tollFreeDates = _feeRepository.GetTollFreeDates();
            
            var calculator = CalculatorFactory.GetCityCalculator(city);
            
            return calculator(passages, maxDayFee, fees, tollFreeDates);
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
    }
}