using Moq;
using Newtonsoft.Json;
using Payspace.Domain;
using Payspace.Domain.Interface;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Payspace.Tests
{
    public class BaseTest
    {
        protected Mock<ITaxTypeToPostalCodeMapper> _taxRegimeMapper = new Mock<ITaxTypeToPostalCodeMapper>();
        protected Mock<IProgressiveTaxRateDataProvider> _taxDataProvier = new Mock<IProgressiveTaxRateDataProvider>();
        private IEnumerable<ProgressiveTaxRate> _taxTable;
        protected ITaxRateCalculator _calculator;
        protected void SetUpData()
        {
            string postCodeContent = File.ReadAllText("./Mock/postalCodes.json");
            var codes = JsonConvert.DeserializeObject<List<PostalCode>>(postCodeContent);
            foreach (var code in codes)
            {
                _taxRegimeMapper.Setup(f => f.GetTaxRegime(code.Code)).Returns(code.TaxType);
            }

            string taxTableContent = File.ReadAllText("./Mock/taxTable.json");
            _taxTable = JsonConvert.DeserializeObject<List<ProgressiveTaxRate>>(taxTableContent);
            _taxDataProvier.Setup(s => s.TaxRates).Returns(_taxTable);
        }
    }
}
