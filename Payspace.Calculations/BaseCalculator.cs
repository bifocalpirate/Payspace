using Payspace.Domain.Interface;
using Payspace.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Payspace.Calculations
{
    public abstract class BaseCalculator : ITaxRateCalculator
    {
        public virtual void Validate(decimal taxableAmount)
        {
            if (taxableAmount < 0)
                throw new InvalidInputAmount($"Value {taxableAmount} is not positive.");
        }
        public abstract decimal GetTaxableAmount(decimal taxableAmount);      
    }
}
