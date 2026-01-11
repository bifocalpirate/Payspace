using Payspace.API.Models;
using Payspace.API.Models.DTO;
using Payspace.Domain;
using Payspace.UI.Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaxCalculationEvent = Payspace.Domain.TaxCalculationEvent;

namespace Payspace.UI.Facades
{
    public interface IRepositoryFacade
    {
        IEnumerable<ProgressiveTaxRate> GetProgressiveTaxRates();
        void UpdateProgressiveTaxRate(ProgressiveTaxRate taxRate);
        void AddProgressiveTaxRate(ProgressiveTaxRate taxRate);
        void DeleteProgressiveTaxRate(long id);
        IEnumerable<PostalCode> GetPostalCodeMappings();
        void DeletePostalCodeMapping(long id);
        void AddPostalCodeMapping(PostalCode postalCode);
        PostalCode GetPostalCodeMapping(string postalCode);
        CalculationResponse CalculateTax(CalculationRequest request);
        IEnumerable<TaxCalculationEvent> GetTaxCalculationEvents();
        AuthenticatedUser LoginUser(string emailAddress,string passwordHash);
        void SetJWTToken(string token);

    }
}
