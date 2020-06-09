using Moq;
using NUnit.Framework;
using Payspace.Calculations;
using Payspace.Domain.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace Payspace.Tests
{
    [TestFixture]
    public class TaxCalculationManagerTests
    {
        Mock<IProgressiveRateCalculator> _progressiveCalculator = new Mock<IProgressiveRateCalculator>();
        Mock<IFlatRateCalculator> _flatRateCalculate = new Mock<IFlatRateCalculator>();
        Mock<IFlatValueCalculator> _flatValueCalculator = new Mock<IFlatValueCalculator>();
        TaxCalculatorManager _manager;

        [SetUp]
        public void Setup() {
            _manager = new TaxCalculatorManager(_progressiveCalculator.Object,
                                               _flatValueCalculator.Object,
                                               _flatRateCalculate.Object); 
        }
        [Test]
        public void When_TaxType_Is_Progressive_Return_ProgressiveCalculator()
        {
            Assert.IsInstanceOf(typeof(IProgressiveRateCalculator), _manager.GetCalculator(Domain.Enums.TaxType.Progressive));
        }
        [Test]
        public void When_TaxType_Is_FlatValue_Return_FlatValueCalculator()
        {
            Assert.IsInstanceOf(typeof(IFlatValueCalculator), _manager.GetCalculator(Domain.Enums.TaxType.FlatValue));
        }
        [Test]
        public void When_TaxType_Is_FlatRate_Return_FlatRateCalculator()
        {
            Assert.IsInstanceOf(typeof(IFlatRateCalculator), _manager.GetCalculator(Domain.Enums.TaxType.FlatRate));
        }
    }
}
