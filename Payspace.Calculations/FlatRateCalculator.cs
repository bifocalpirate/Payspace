using Payspace.Domain.Enums;
using Payspace.Domain.Interface;
using Payspace.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Payspace.Calculations
{
    
    public class FlatRateCalculator : BaseCalculator, IFlatRateCalculator 
    {
        private readonly decimal _flatRate;

        public FlatRateCalculator(decimal flatRate)
        {
            if (flatRate < 0)
                throw new InvalidInputAmount($"Cannot instantiate the FlatRateCalculator with a negative rate '{flatRate}'");
            _flatRate = flatRate;
        }
        public override decimal GetTaxableAmount(decimal taxableAmount)
        {
            Validate(taxableAmount);
            //double math is extremely fast, but has less precision than decimal
            return _flatRate * taxableAmount;
        }
    }
}
