using Payspace.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Payspace.Domain
{
    public class PostalCode:BaseEntity<long>
    {
        public string Code { get; set; }
        public TaxType TaxType { get; set; }
    }
}
