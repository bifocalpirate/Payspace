using Microsoft.Extensions.Options;
using Payspace.API.Models;
using Payspace.API.Models.DTO;
using Payspace.Domain;
using Payspace.Domain.Configuration;
using Payspace.Messaging.Http;
using Payspace.UI.Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;


namespace Payspace.UI.Facades
{
    public class APIFacada : IRepositoryFacade
    {
        private readonly IMessager _messager;
        private readonly IOptions<AppSettings> _settings;
        private string _jwtToken;
        
        public APIFacada(IMessager messager, IOptions<AppSettings> settings, string jwtToken=null)
        {
            _messager = messager;
            _settings = settings;
            _jwtToken = jwtToken;
        }
        public void AddPostalCodeMapping(PostalCode postalCode)
        {
            throw new NotImplementedException();
        }

        public void DeletePostalCodeMapping(long id)
        {
            throw new NotImplementedException();
        }

        public void DeleteProgressiveTaxRate(long id)
        {
           var result = _messager.Delete<string>(GetRemoteUrl("ProgressiveTaxRate", id.ToString()),_jwtToken).Result;
        }

        public IEnumerable<PostalCode> GetPostalCodeMappings()
        {
            return _messager.Get<IEnumerable<PostalCode>>(GetRemoteUrl("PostalCode"), _jwtToken).Result;
        }
        public CalculationResponse CalculateTax(CalculationRequest request)
        {
            return _messager.Post<CalculationRequest,CalculationResponse>(GetRemoteUrl("Calculator"), request, _jwtToken).Result;
        }
        public PostalCode GetPostalCodeMapping(string postalCode)
        {
            return _messager.Get<PostalCode>(GetRemoteUrl("PostalCode", postalCode), _jwtToken).Result;
        }

        public IEnumerable<ProgressiveTaxRate> GetProgressiveTaxRates()
        {
            return _messager.Get<IEnumerable<ProgressiveTaxRate>>(GetRemoteUrl("ProgressiveTaxRate"), _jwtToken).Result;
        }

        public IEnumerable<Domain.TaxCalculationEvent> GetTaxCalculationEvents()
        {
            return _messager.Get<IEnumerable<Domain.TaxCalculationEvent>>(GetRemoteUrl("TaxEvent"), _jwtToken).Result;
        }
        public void UpdateProgressiveTaxRate(ProgressiveTaxRate taxRate)
        {
            var result = _messager.Put<ProgressiveTaxRate, string>(GetRemoteUrl("ProgressiveTaxRate"), taxRate, _jwtToken).Result;
        }
        public void AddProgressiveTaxRate(ProgressiveTaxRate taxRate)
        {
            var result = _messager.Post<ProgressiveTaxRate, string>(GetRemoteUrl("ProgressiveTaxRate"), taxRate, _jwtToken).Result;
        }
        protected string GetRemoteUrl(string url, params string[] parameters)
        {
            var p = string.Join("/", parameters);
            if (parameters.Any())
            {
                return $"{_settings.Value.APIUrl}/{url}/{p}";
            }
            return $"{_settings.Value.APIUrl}/{url}";
        }  
        

        public AuthenticatedUser LoginUser(string emailAddress, string passwordHash)
        {
            return _messager.Post<UserForAuthentication, AuthenticatedUser>(GetRemoteUrl("Login"), new UserForAuthentication()
            {
                EmailAddress = emailAddress,
                PasswordHash = passwordHash
            }).Result;
        }

        public void SetJWTToken(string token)
        {
            _jwtToken = token;
        }
    }
}
