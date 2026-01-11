using Payspace.Domain.Enums;
using Payspace.Domain.Interface;
using Payspace.Domain;
using Payspace.Exceptions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace Payspace.Calculations
{
    public class ProgressiveRateCalculator : BaseCalculator, IProgressiveRateCalculator
    {
        //note the use of the readonly collection
        private IReadOnlyCollection<ProgressiveTaxRate> _taxTable;
        private readonly IProgressiveTaxRateDataProvider _dataProvider;

        public ProgressiveRateCalculator(IProgressiveTaxRateDataProvider dataProvider)
        {
            _dataProvider = dataProvider;
        }
        
        public void RefreshTaxTable(IEnumerable<ProgressiveTaxRate> taxTable)
        {
            //you'll want to log this refresh or not use it at all
            taxTable = taxTable.OrderBy(x => x.From);
            _taxTable = new ReadOnlyCollection<ProgressiveTaxRate>(taxTable.ToList());
        }
        public override void Validate(decimal taxableAmount)
        {
            base.Validate(taxableAmount);
            //we need to check that the tax table is valid (no overlaps)
        }


        public override decimal GetTaxableAmount(decimal taxableAmount)
        {
            Validate(taxableAmount);
            RefreshTaxTable();
            var runningTotalForTax = 0M;
            var applicableSchedule = _taxTable.Where(x => x.From <= taxableAmount);
            if (!applicableSchedule.Any())
            {
                //no tax rate could be found, maybe log this?
                return 0M;
            }
            foreach(var taxRate in applicableSchedule)
            {
                //catch the edge case where the salary is falls right on the From of the bracket.
                var taxing = Math.Min(taxableAmount, taxRate.To == 0 ? taxableAmount : taxRate.To) -
                                       taxRate.From + (taxRate.From == taxableAmount?1:0);
                runningTotalForTax += (taxing) * taxRate.Rate;
            }
            return runningTotalForTax;
        }
        private void RefreshTaxTable()
        {
            //you want to cache this data and only update it infrequently!
            var taxTable = _dataProvider.TaxRates;
            if (taxTable == null || !taxTable.Any())
                throw new InvalidTaxTable("The taxtable is either null or empty.");
            taxTable = taxTable.OrderBy(x => x.From);
            _taxTable = new ReadOnlyCollection<ProgressiveTaxRate>(taxTable.ToList());
        }
    }
}
