using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

//ideally these artefacts should go into their own projects as well.
namespace Payspace.Domain.Enums
{
    public enum TaxType
    {
        Progressive = 0,
        FlatValue = 1,
        FlatRate = 2,
        Unknown = 3
    }
}
