using System;
using System.Collections.Generic;
using System.Text;

namespace Payspace.Domain.Interface
{
    public interface IFlatRateCalculator:ITaxRateCalculator {}
    public interface IFlatValueCalculator:ITaxRateCalculator {}
    public interface IProgressiveRateCalculator:ITaxRateCalculator {}
}
