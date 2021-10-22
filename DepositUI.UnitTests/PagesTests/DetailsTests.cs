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
using System;
using System.Collections.Generic;
using System.Text;

namespace DepositUI.UnitTests.PagesTests
{
    public class DetailsTests
    {
        private List<DepositCalculation> depositCalculations;
        private Mock<IDepositService> serviceMock;

        [SetUp]
        public void SetUp()
        {
            this.depositCalculations = ModelsProvider.DepositCalculationList;
            this.serviceMock = new Mock<IDepositService>();
            this.serviceMock.Setup(s => s.GetDepositDetailsAsync(It.IsAny<int>())).ReturnsAsync(this.depositCalculations);
        }

        [Test]
        public void ComponentRender_WithoutAuthorization_ExpectRedirectToLogin()
        {
            using var ctx = new Bunit.TestContext();
            ctx.Services.AddScoped(s => this.serviceMock.Object);
            ctx.AddTestAuthorization();
            ctx.JSInterop.Mode = JSRuntimeMode.Loose;
            var navigation = ctx.Services.GetRequiredService<NavigationManager>();

            var cut = ctx.RenderComponent<Details>(opt => opt.Add(c => c.DepositId, 0));

            Assert.IsTrue(navigation.Uri.Contains("authentication/login"));
        }

        [Test]
        public void ComponentRender_WithAuthorization_ExpectListDepositCalculation()
        {
            using var ctx = new Bunit.TestContext();
            ctx.Services.AddScoped(s => this.serviceMock.Object);
            var authContext = ctx.AddTestAuthorization();
            authContext.SetAuthorized("User");
            ctx.JSInterop.Mode = JSRuntimeMode.Loose;
            var navigation = ctx.Services.GetRequiredService<NavigationManager>();

            var cut = ctx.RenderComponent<Details>(opt => opt.Add(c => c.DepositId, 0));

            Assert.AreEqual(this.depositCalculations, cut.Instance.DepositDetails);
            this.serviceMock.Verify(s => s.GetDepositDetailsAsync(It.Is<int>(i => i == 0)));
        }

        [Test]
        public void ComponentRender_GetCSV_WithAuthorization_ExpectListDepositCalculation()
        {
            using var ctx = new Bunit.TestContext();
            ctx.Services.AddScoped(s => this.serviceMock.Object);
            var authContext = ctx.AddTestAuthorization();
            authContext.SetAuthorized("User");
            ctx.JSInterop.Mode = JSRuntimeMode.Loose;
            var navigation = ctx.Services.GetRequiredService<NavigationManager>();

            var cut = ctx.RenderComponent<Details>(opt => opt.Add(c => c.DepositId, 0));
            var button = cut.Find("button");
            button.Click();

            serviceMock.Verify(s => s.GetDepositCSVAsync(It.Is<int>(i => i == 0)));
        }
    }
}
