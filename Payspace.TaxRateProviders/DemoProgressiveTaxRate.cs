using Payspace.Domain.Interface;
using Payspace.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Payspace.TaxRateProviders
{
    public class DemoProgressiveTaxRate : IProgressiveTaxRateDataProvider
    {
        public IEnumerable<ProgressiveTaxRate> TaxRates => throw new NotImplementedException();
    }
}
