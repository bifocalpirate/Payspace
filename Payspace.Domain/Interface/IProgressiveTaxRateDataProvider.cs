using Payspace.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Payspace.Domain.Interface
{
    public interface IProgressiveTaxRateDataProvider
    {
        IEnumerable<ProgressiveTaxRate> TaxRates { get; }
    }
}
