using Payspace.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Payspace.Domain.Interface
{
    public interface ITaxRateCalculator
    {
        decimal GetTaxableAmount(decimal taxableAmount);
    }
}
