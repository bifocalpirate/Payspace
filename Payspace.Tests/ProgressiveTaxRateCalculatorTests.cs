using NUnit.Framework;
using Payspace.Calculations;
using Payspace.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Payspace.Tests
{
    [TestFixture]
    public class ProgressiveTaxRateCalculatorTests:BaseTest
    {
        
        [SetUp]
        public void SetUp()
        {
            SetUpData();
            _calculator = new ProgressiveRateCalculator(_taxDataProvier.Object);
        }
        [Test]
        public void When_Taxable_Is_8350()
        {
            var taxed = 8350 * 0.1;
            Assert.AreEqual(taxed,_calculator.GetTaxableAmount(8350));
        }

        [Test]
        public void When_Taxable_Is_8351()
        {
            var taxed1 = 8350 * 0.1;
            var taxed2 = 1.0 * 0.15;
            Assert.AreEqual(taxed1+taxed2, _calculator.GetTaxableAmount(8351));
        }
        [Test]
        public void When_Taxable_is_16700()
        {
            Assert.AreEqual(2087.35M, _calculator.GetTaxableAmount(16700));
        }

        [Test]
        public void When_Taxable_Is_9000()
        {
            var taxed1 = 8350 * 0.1;
            var taxed2 = (9000-8351) * 0.15;

            Assert.AreEqual(taxed1+taxed2, _calculator.GetTaxableAmount(9000));
        }
        [Test]
        public void When_Taxable_Is_40000()
        {
            Assert.AreEqual(6187.10M, _calculator.GetTaxableAmount(40000));
        }

        //any further tests really either test the validity of the tax table (overlaps, gaps)
        //and the theory of numbers itself. i would add more tax table tests especially
        //around what happens with edge cases.
    }
}
