using Moq;
using Newtonsoft.Json;
using NUnit.Framework;
using Payspace.Calculations;
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
    public class FlatValueCalculatorTests:BaseTest
    {        
   
        [SetUp]
        public void SetUp()
        {
            SetUpData();
            _calculator = new FlatValueCalculator(10000M, 200000M, 0.05M); 
        }
        [Test]
        public void When_Taxable_Is_More_Than_200K_Rate_Is_10k()
        {
            Assert.AreEqual(10000, _calculator.GetTaxableAmount(205000), "Expected a tax amount of 10k for amount 205k");
        }
        [Test]
        public void When_Taxable_Is_Equal_To_200K_Rate_Is_10k()
        {
            //edge case test
            Assert.AreEqual(10000, _calculator.GetTaxableAmount(200000), "Expected a tax amount of 10k for amount of 200k");
        }

        [Test]
        public void When_Taxable_Is_Less_Than_200K_Rate_Is_5pc()
        {
            Assert.AreEqual(5000, _calculator.GetTaxableAmount(100000), "Expected a tax amount of 5k for amount less than 200k");
        }
    }
}
