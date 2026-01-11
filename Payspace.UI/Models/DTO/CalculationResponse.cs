using Payspace.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Payspace.API.Models.DTO
{
    public class CalculationResponse
    {
        public decimal AssessedAmount { get; set; }
        public TaxType TaxType { get; set; }
        public string ErrorMessage { get; set; }  
    }
}
