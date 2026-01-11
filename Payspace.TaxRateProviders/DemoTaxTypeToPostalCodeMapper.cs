using Payspace.Domain.Enums;
using Payspace.Domain.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace Payspace.TaxRateProviders
{
    public class DemoTaxTypeToPostalCodeMapper : ITaxTypeToPostalCodeMapper
    {
        public TaxType GetTaxRegime(string postalCode)
        {
            throw new NotImplementedException();
        }
    }
}
