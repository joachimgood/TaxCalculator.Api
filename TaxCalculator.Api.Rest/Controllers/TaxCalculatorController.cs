using Microsoft.AspNetCore.Mvc;
using TaxCalculator.Api.Core.Interfaces;
using TaxCalculator.Api.Rest.Validation;

namespace TaxCalculator.Api.Rest.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TaxCalculatorController : ControllerBase
    {
        private readonly ITaxCalculatorService _taxCalculatorService;

        public TaxCalculatorController(ITaxCalculatorService taxCalculatorService)
        {
            _taxCalculatorService = taxCalculatorService;
        }

        /// <summary>
        /// Get total tax of passages (please be advise only datetime with year 2013, and city "Gothenburg" is currently implemented)
        /// </summary>
        /// <returns></returns>
        [HttpPost(Name = "CalculateTax")]
        public IActionResult CalculateTax([FromBody] CalculateTaxRequest request)
        {
            var taxFee = _taxCalculatorService.GetTax(request.Vehicle, request.Passages, request.City);
            return Ok(taxFee);
        }
    }


    //Todo: move this to other file or 'Contract-project'
    public class CalculateTaxRequest
    {
        [VehicleTypeValidation]
        public string Vehicle { get; set; }

        [CityValidation]
        public string City { get; set; }

        [DateInRangeValidation("2013-01-01", "2013-12-31")]
        public DateTime[] Passages { get; set; }
    }
}