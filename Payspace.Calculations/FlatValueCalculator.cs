using Payspace.Domain.Enums;
using Payspace.Domain.Interface;
using Payspace.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Payspace.Calculations
{
    public class FlatValueCalculator : BaseCalculator,IFlatValueCalculator
    {
        private readonly decimal _flatValue;
        private readonly decimal _appliesAt;
        private readonly decimal _rate;

        public FlatValueCalculator(decimal flatValue, decimal appliesAt, decimal rate)
        {
            if (flatValue < 0 || rate < 0)
                throw new InvalidInputAmount($"Cannot instantiate the FlatValueCalculator with a negative rate '{rate}' or base value '{flatValue}'");

            _flatValue = flatValue;
            _appliesAt = appliesAt;
            _rate = rate;
        }
        public override decimal GetTaxableAmount(decimal taxableAmount)
        {
            Validate(taxableAmount);
            if (taxableAmount < _appliesAt)
                return taxableAmount * _rate;
            return _flatValue;
        }
    }
}
