using Bunit;
using Bunit.TestDoubles;
using DepositUI.Core.Data;
using DepositUI.Core.Interfaces;
using DepositUI.Pages;
using DepositUI.UnitTests.Utility;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;

namespace DepositUI.UnitTests.PagesTests
{
    [TestFixture]
    class HistoryTests
    {
        private List<DepositModel> depositModels;
        private Mock<IDepositService> serviceMock;

        [SetUp]
        public void SetUp()
        {
            this.depositModels = ModelsProvider.DepositModelList;
            this.serviceMock = new Mock<IDepositService>();
            this.serviceMock.Setup(s => s.GetDepositsAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(this.depositModels);
        }

        [Test]
        public void ComponentRender_WithoutAuthorization_ExpectRedirectToLogin()
        {
            using var ctx = new Bunit.TestContext();
            ctx.Services.AddScoped(s => this.serviceMock.Object);
            ctx.AddTestAuthorization();
            var navigation = ctx.Services.GetRequiredService<NavigationManager>();

            var cut = ctx.RenderComponent<History>();

            Assert.IsTrue(navigation.Uri.Contains("authentication/login"));
        }

        [Test]
        public void ComponentRender_WithAuthorization_ExpectListDepositModel()
        {
            using (var ctx = new Bunit.TestContext())
            {
                ctx.Services.AddScoped(s => this.serviceMock.Object);
                var authContext = ctx.AddTestAuthorization();
                authContext.SetAuthorized("User");

                var cut = ctx.RenderComponent<History>();

                this.serviceMock.Verify(s => s.GetDepositsAsync(It.Is<int>(i => i == 0), It.Is<int>(i => i == 16)));
                var columns = cut.FindAll("div.col-3");
                Assert.AreEqual(1, columns[0].ChildElementCount);
                Assert.AreEqual(1, columns[1].ChildElementCount);
                Assert.AreEqual(0, columns[2].ChildElementCount);
                Assert.AreEqual(0, columns[3].ChildElementCount);

                var model = ModelsProvider.DepositModelList;
                var values = cut.FindAll("div.align-right");
                Assert.AreEqual(2, values.Count);
                Assert.AreEqual($"{model[0].Percent}%", values[0].Children[0].TextContent);
                Assert.AreEqual($"{model[0].Term}", values[0].Children[2].TextContent);
                Assert.AreEqual($"{model[0].Amount}$", values[0].Children[4].TextContent);
                Assert.AreEqual($"{model[0].Date:dd.MM.yyyy}", values[0].Children[6].TextContent);

                Assert.AreEqual($"{model[1].Percent}%", values[1].Children[0].TextContent);
                Assert.AreEqual($"{model[1].Term}", values[1].Children[2].TextContent);
                Assert.AreEqual($"{model[1].Amount}$", values[1].Children[4].TextContent);
                Assert.AreEqual($"{model[1].Date:dd.MM.yyyy}", values[1].Children[6].TextContent);
            }
        }

        [Test]
        public void ComponentRender_LoadMore_WithAuthorization_ExpectListDepositModel()
        {
            using (var ctx = new Bunit.TestContext())
            {
                ctx.Services.AddScoped(s => this.serviceMock.Object);
                var authContext = ctx.AddTestAuthorization();
                authContext.SetAuthorized("User");

                var cut = ctx.RenderComponent<History>();
                var buttons = cut.FindAll("button");
                buttons[2].Click();

                this.serviceMock.Verify(s => s.GetDepositsAsync(It.Is<int>(i => i == 2), It.Is<int>(i => i == 16)));
                var columns = cut.FindAll("div.col-3");
                Assert.AreEqual(1, columns[0].ChildElementCount);
                Assert.AreEqual(1, columns[1].ChildElementCount);
                Assert.AreEqual(1, columns[2].ChildElementCount);
                Assert.AreEqual(1, columns[3].ChildElementCount);

                var values = cut.FindAll("div.align-right");
                Assert.AreEqual(4, values.Count);
            }
        }
    }
}
