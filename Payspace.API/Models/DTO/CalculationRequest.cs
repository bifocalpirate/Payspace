using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Payspace.API.Models
{
    public class CalculationRequest
    {
        public string PostalCode { get; set; }
        public decimal TaxableAmount { get; set; }
        public long? UserId { get; set; }
    }
}
