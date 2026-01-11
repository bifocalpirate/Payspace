using Payspace.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Payspace.Domain.Interface
{
    public interface ITaxTypeToPostalCodeMapper
    {
        TaxType GetTaxRegime(string postalCode);
    }
}
