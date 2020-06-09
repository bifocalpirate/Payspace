using Payspace.Domain.Enums;
using Payspace.Domain.Interface;
using Payspace.Exceptions;

namespace Payspace.Calculations
{
    public class TaxCalculatorManager 
    {
        private readonly IProgressiveRateCalculator _progressiveCalculator;
        private readonly IFlatValueCalculator _flatValueCalculator;
        private readonly IFlatRateCalculator _flatRateCalculator;

        public TaxCalculatorManager(IProgressiveRateCalculator progressiveCalculator, 
            IFlatValueCalculator flatValueCalculator, 
            IFlatRateCalculator flatRateCalculator)
        {
           //the calculators are cheap to keep and instantiate in memory (they're scoped to the request)
           //but you might want to cache the TaxTable Provider Data as that would be better
           //to keep around for longer. the provider should be implemented as a singleton 
           //with a data cache. you don't want to keep hitting the db on every request
           //the progressiveCalculator can have its taxtable refreshed tho, so may implement
           //with an empty list and at least once during its lifetime load with a real table.
            _progressiveCalculator = progressiveCalculator;
            _flatValueCalculator = flatValueCalculator;
            _flatRateCalculator = flatRateCalculator;
        }       
        
        public ITaxRateCalculator GetCalculator(TaxType taxType)
        {
            switch (taxType)
            {
                case TaxType.FlatRate:
                    return _flatRateCalculator; // new FlatRateCalculator(0.175M); //should read this from config actually. Due to time constraints we will hardcode.
                case TaxType.FlatValue:
                    return _flatValueCalculator; //new FlatValueCalculator(10000M, 200000M, 0.05M); //same
                case TaxType.Progressive:
                    return _progressiveCalculator; //new ProgressiveRateCalculator(taxTable);
                default:
                    throw new UnsupportedTaxTaxRegime($"{taxType} is not configured for instantiation.");
            }
        }
    }
}
