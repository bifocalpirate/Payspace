using System;
using System.Collections.Generic;
using System.Text;

namespace Payspace.Domain.Interface
{
    public interface ITaxCalculationEventLogger
    {
        void LogEvent(TaxCalculationEvent taxEvent);
    }
}
