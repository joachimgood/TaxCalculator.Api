using TaxCalculator.Api.Data.Interfaces;
using TaxCalculator.Api.Data.Entities;

namespace TaxCalculator.Api.Data.Repositories
{
    public class VehicleRepository : IVehicleRepository
    {
        private readonly List<Vehicle> vehicles;

        public VehicleRepository()
        {
            vehicles = new List<Vehicle>
            {
                new Vehicle { Type = VehicleType.PrivateCar, IsTollFree = false },
                new Vehicle { Type = VehicleType.MotorCycle, IsTollFree = true },
                new Vehicle { Type = VehicleType.Tractor, IsTollFree = false },
                new Vehicle { Type = VehicleType.Emergency, IsTollFree = true },
                new Vehicle { Type = VehicleType.Diplomat, IsTollFree = true },
                new Vehicle { Type = VehicleType.ForeignCar, IsTollFree = true },
                new Vehicle { Type = VehicleType.Military, IsTollFree = true },
                new Vehicle { Type = VehicleType.Bus, IsTollFree = true },
            };
        }

        public List<Vehicle> GetAllVehicles()
        {
            return vehicles.ToList();
        }

        public Vehicle GetVehicleByType(VehicleType type)
        {

            var vehicle = vehicles.FirstOrDefault(x => x.Type == type);

            if (vehicle == null) { throw new Exception("Vehicle not found"); }

            return vehicle;
        }
    }
}
