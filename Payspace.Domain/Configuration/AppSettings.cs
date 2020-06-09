using System;
using System.Collections.Generic;
using System.Text;
using System.Transactions;

namespace Payspace.Domain.Configuration
{
    public class AppSettings
    {
        public int TokenTimeOutInHours { get; set; }
        public string AuthenticationAPIUrl { get; set; }
        public string APIUrl { get; set; }
        public string ClientIdentifier { get; set; }
        public decimal FlatRateRate { get; set; }
        public decimal FlatValue { get; set; }
        public decimal FlatValueTrigger { get; set; }
        public decimal FlatValuePercentage { get; set; }    
    }
}
