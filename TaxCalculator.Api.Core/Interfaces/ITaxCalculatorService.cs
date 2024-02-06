
using TaxCalculator.Api.Data.Entities;

namespace TaxCalculator.Api.Core.Interfaces
{
    public interface ITaxCalculatorService
    {
        int GetTax(string typeOfVehicle, DateTime[] passages, string city);
    }
}
