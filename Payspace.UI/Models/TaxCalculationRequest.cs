using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Payspace.UI.Models.DTO
{
    public class TaxCalculationRequest
    {
        public long? UserId {get;set;}
        [Range(0, double.MaxValue, ErrorMessage = "Please enter valid positive decimal number.")]
        public decimal TaxableAmount { get; set; }
        [RegularExpression("[A-Z,0-9]{4}", ErrorMessage ="Postal code should be a 4 character alphanumeric string.")]
        public string PostalCode { get; set; }
        public string Note { get; set; }
    }
}
