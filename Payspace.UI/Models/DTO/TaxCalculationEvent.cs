using Payspace.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Payspace.UI.Models.DTO
{
    public class TaxCalculationEvent
    {
        public long? UserId { get; set; }
        public decimal AssessedAmount { get; set; }
        public decimal TaxableAmount { get; set; }
        public string PostalCode { get; set; }     
        public TaxType TaxRegime { get; set; }
   
    }
}
