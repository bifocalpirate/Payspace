using Payspace.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Payspace.Domain
{
    public class TaxCalculationEvent:BaseEntity<long>
    {
        public long? UserId { get; set; }
        public decimal AssessedAmount { get; set; }
        public decimal TaxableAmount { get; set; }
        public string PostalCode { get; set; }
        public TaxType TaxRegime { get; set; }  

    }
}
