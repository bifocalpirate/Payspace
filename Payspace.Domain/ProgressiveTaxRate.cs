using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Payspace.Domain
{
    public class ProgressiveTaxRate:BaseEntity<long>
    {
        public decimal Rate { get; set; } 
        public decimal From { get; set; }
        public decimal To { get; set; }
    }
}
