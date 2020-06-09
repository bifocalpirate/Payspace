using Payspace.API.Models;
using Payspace.Domain.Enums;
using Payspace.Domain.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace Payspace.API.Respositories
{
    public class PostCodeToTaxTypeProvider : ITaxTypeToPostalCodeMapper
    {
        private readonly AppDbContext _dbContext;

        public PostCodeToTaxTypeProvider(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public TaxType GetTaxRegime(string postalCode)
        {
            var postCode = _dbContext.PostalCodeMappings.FirstOrDefault(x => x.Code == postalCode);
            if (postCode != null)
                return postCode.TaxType;
            return TaxType.Unknown;
        }
    }
}
