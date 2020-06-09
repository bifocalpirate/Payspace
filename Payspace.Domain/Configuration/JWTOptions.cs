using System;
using System.Collections.Generic;
using System.Text;

namespace Payspace.Domain.Configuration
{
    public class JWTOptions
    {
        public string Key { get; set; }
        public string Issuer { get; set; }
    }
}
