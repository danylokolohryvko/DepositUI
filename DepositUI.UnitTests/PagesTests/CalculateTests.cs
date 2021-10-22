using Bunit;
using DepositUI.BLL.Services;
using DepositUI.Core.Data;
using DepositUI.Core.Interfaces;
using DepositUI.Pages;
using DepositUI.UnitTests.Utility;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;

namespace DepositUI.UnitTests.PagesTests
{
    [TestFixture]
    class CalculateTests
    {
        public Mock<IDepositService> serviceMock;
        public List<DepositCalculation> depositCalculations;

        [SetUp]
        public void SetUp()
        {
            this.depositCalculations = ModelsProvider.DepositCalculationList;
            this.serviceMock = new Mock<IDepositService>();
            this.serviceMock.Setup(s => s.CalculateDepositAsync(It.IsAny<DepositModel>())).ReturnsAsync(this.depositCalculations);
        }

        [Test]
        public void CalculateDeposite_ValidForm_ExpectListDepositCalculation()
        {
            using var ctx = new Bunit.TestContext();
            ctx.Services.AddScoped(s => this.serviceMock.Object);
            ctx.JSInterop.Mode = JSRuntimeMode.Loose;
            var cut = ctx.RenderComponent<Calculate>();

            cut.Find("input.percent").Change("5");
            cut.Find("input.term").Change("5");
            cut.Find("input.amount").Change("5");
            cut.Find("form").Submit();

            var item = cut.Instance.DepositDetails;
            var model = cut.Instance.Deposit;
            Assert.AreEqual(this.depositCalculations, item);
            this.serviceMock.Verify(s => s.CalculateDepositAsync(It.Is<DepositModel>(m => m == model)));
        }

        [Test]
        public void CalculateDeposit_InvalidForm_ExpectNull()
        {
            using var ctx = new Bunit.TestContext();
            ctx.Services.AddScoped(s => this.serviceMock.Object);
            ctx.JSInterop.Mode = JSRuntimeMode.Loose;
            var cut = ctx.RenderComponent<Calculate>();

            cut.Find("form").Submit();

            var item = cut.Instance.DepositDetails;
            Assert.IsNull(item);
        }
    }
}
