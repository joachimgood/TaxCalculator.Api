﻿
namespace TaxCalculator.Api.Core.Interfaces
{
    public interface ITaxCalculatorService
    {
        int GetTax(string vehicleType, DateTime[] passages, string city);
    }
}
