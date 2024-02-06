using TaxCalculator.Api.Data.Entities;

namespace TaxCalculator.Api.Data.Interfaces
{
    public interface IVehicleRepository
    {
        public List<Vehicle> GetAllVehicles();
        public Vehicle GetVehicleByType(VehicleType type);

    }
}
