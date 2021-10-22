using Bunit;
using DepositUI.Components;
using DepositUI.Core.Data;
using DepositUI.UnitTests.Utility;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace DepositUI.UnitTests.ComponentTests
{
    [TestFixture]
    public class DepositDetailsTableTests
    {
        private List<DepositCalculation> depositCalculations;

        [SetUp]
        public void SetUp()
        {
            this.depositCalculations = ModelsProvider.DepositCalculationList;
        }

        [Test]
        public void RenderComponent_ExpextTwoRowsInTable()
        {
            using var ctx = new Bunit.TestContext();
            ctx.JSInterop.Mode = JSRuntimeMode.Loose;
            var cut = ctx.RenderComponent<DepositDetailsTable>(opt => opt.Add(c => c.DepositDetails, depositCalculations));

            var trs = cut.FindAll("tbody > tr");

            Assert.AreEqual(depositCalculations.Count, trs.Count);

            Assert.AreEqual(depositCalculations[0].Month.ToString(), trs[0].Children[0].TextContent);
            Assert.AreEqual(depositCalculations[0].PercentAdded.ToString(), trs[0].Children[1].TextContent);
            Assert.AreEqual(depositCalculations[0].TotalAmount.ToString(), trs[0].Children[2].TextContent);

            Assert.AreEqual(depositCalculations[1].Month.ToString(), trs[1].Children[0].TextContent);
            Assert.AreEqual(depositCalculations[1].PercentAdded.ToString(), trs[1].Children[1].TextContent);
            Assert.AreEqual(depositCalculations[1].TotalAmount.ToString(), trs[1].Children[2].TextContent);
        }

        [Test]
        public void RenderComponent_ExpextNoRowsInTable()
        {
            using var ctx = new Bunit.TestContext();
            ctx.JSInterop.Mode = JSRuntimeMode.Loose;
            List<DepositCalculation> model = null;
            var cut = ctx.RenderComponent<DepositDetailsTable>(opt => opt.Add(c => c.DepositDetails, model));

            var trs = cut.FindAll("tbody > tr");

            Assert.AreEqual(0, trs.Count);
        }
    }
}
