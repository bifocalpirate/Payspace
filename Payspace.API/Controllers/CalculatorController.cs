using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Payspace.API.Models;
using Payspace.API.Models.DTO;
using Payspace.Calculations;
using Payspace.Domain.Enums;
using Payspace.Domain.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Payspace.API.Controllers
{
    public class CalculatorController:BaseController
    {
        private readonly TaxCalculatorManager _taxManager;
        private readonly ITaxTypeToPostalCodeMapper _postalCodeToTaxTypeMapper;
        private readonly ITaxCalculationEventLogger _taxEventLogger;
        ILogger<CalculatorController> _logger;
        public CalculatorController(TaxCalculatorManager taxManager, 
            ITaxTypeToPostalCodeMapper postalCodeToTaxTypeManager,
            ITaxCalculationEventLogger taxEventLogger,
            ILogger<CalculatorController> logger)
        {
            _taxManager = taxManager;
            _postalCodeToTaxTypeMapper = postalCodeToTaxTypeManager;
            _taxEventLogger = taxEventLogger;
            _logger = logger;
        }
        [HttpPost]
        [AllowAnonymous]
        public ActionResult<CalculationResponse> CalculateTax([FromBody] CalculationRequest request)
        {
            var taxType = _postalCodeToTaxTypeMapper.GetTaxRegime(request.PostalCode);
            var response = new CalculationResponse();
            if (taxType == TaxType.Unknown)
            {
                var errorMessage = $"Postal code {request.PostalCode} could not be mapped to a valid tax type.";
                response.ErrorMessage = errorMessage;
                _logger.LogWarning(errorMessage);
                
            }
            else {
                var taxDue = _taxManager.GetCalculator(taxType).GetTaxableAmount(request.TaxableAmount);
                response.AssessedAmount = taxDue;
                response.TaxType = taxType;
                //i wanted to use Automap to handle this mapping but no time for that and this mapping is only
                //done here anyway
                _taxEventLogger.LogEvent(new Domain.TaxCalculationEvent()
                {
                    AssessedAmount = response.AssessedAmount,
                    PostalCode = request.PostalCode,
                    TaxRegime = taxType,                   
                    TaxableAmount = request.TaxableAmount,
                    UserId = request.UserId
                });
            }
            return response;
        }
    }
}
