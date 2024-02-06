using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
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
            if (!ModelState.IsValid || !RequestValidator.ValidateCalculateTaxRequest(request))
            {
                return BadRequest(ModelState);
            }

            var taxFee = _taxCalculatorService.GetTax(request.Vehicle, request.Passages, request.City);
            
            return Ok(taxFee);
        }
    }


    //Todo: move this to other file or 'Contract-project'
    public class CalculateTaxRequest
    {
        [Required]
        public string Vehicle { get; set; }

        [Required]
        public string City { get; set; }

        [Required]
        public DateTime[] Passages { get; set; }
    }
}