using Moq;
using Newtonsoft.Json;
using NUnit.Framework;
using Payspace.Domain;
using Payspace.Domain.Enums;
using Payspace.Domain.Interface;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Payspace.Tests
{
    [TestFixture]
    public class TaxTypeToPostalCodeMapperTests
    {
        Mock<ITaxTypeToPostalCodeMapper> _taxRegimeMapper = new Mock<ITaxTypeToPostalCodeMapper>();
        
        [SetUp]
        public void Setup()
        {
            var postCodeContent = File.ReadAllText("./Mock/postalCodes.json");
            var codes = JsonConvert.DeserializeObject<List<PostalCode>>(postCodeContent);
            foreach (var code in codes)
            {
                _taxRegimeMapper.Setup(f => f.GetTaxRegime(code.Code)).Returns(code.TaxType);
            }
        }


        [Test]
        public void When_PostalCode_Is_7441_Regime_Is_Progressive()
        {
            Assert.AreEqual(TaxType.Progressive, _taxRegimeMapper.Object.GetTaxRegime("7441"), "Expecting ProgressiveTaxRate for code 7441");
        }

        [Test]
        public void When_PostalCode_Is_A100_Regime_Is_FlatValue()
        {
            Assert.AreEqual(TaxType.FlatValue, _taxRegimeMapper.Object.GetTaxRegime("A100"), "Expecting FlatValue for code A100");
        }

        [Test]
        public void When_PostalCode_Is_7000_Regime_Is_FlatRate()
        {
            Assert.AreEqual(TaxType.FlatRate, _taxRegimeMapper.Object.GetTaxRegime("7000"), "Expecting FlatRate for code 7000");
        }

        [Test]
        public void When_PostalCode_Is_1000_Regime_Is_Progressive()
        {
            Assert.AreEqual(TaxType.Progressive, _taxRegimeMapper.Object.GetTaxRegime("1000"), "Expecting Progressive for code A100");
        }
    }
}
