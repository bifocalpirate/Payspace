using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Text.Json.Serialization;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;
using Payspace.Calculations;
using Payspace.Domain;
using Payspace.Domain.Enums;
using Payspace.Domain.Interface;
using Payspace.Tests;

namespace Payspace.Tests
{
    [TestFixture]
    public class FlatRateCalculatorTests:BaseTest
    {       
  
        [SetUp]
        public void SetUp()
        {
            SetUpData();
            _calculator = new FlatRateCalculator(0.175M);
        }
        [Test]
        public void When_Taxable_Is_100K_Rate_Is_175BasisPoints()
        {
            Assert.AreEqual(17500, _calculator.GetTaxableAmount(100000), "Expected a tax amount of 17500k for amount of 100k");
        }
        [Test]
        public void When_Taxable_Is_AnyPositiveRandom_Rate_Is_175BasisPoints()
        {
            const int PRECISION = 5;
            var taxable = new Random().NextDouble() * new Random().Next(500000);
            var taxed = Math.Round(0.175 * taxable, PRECISION);
            Assert.AreEqual(taxed, Math.Round(_calculator.GetTaxableAmount((decimal) taxable),PRECISION), $"Expected a tax amount of {taxed} for amount of {taxable}");
        }

    }
}
