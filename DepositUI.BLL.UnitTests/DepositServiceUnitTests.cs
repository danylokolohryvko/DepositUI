using DepositUI.BLL.Services;
using DepositUI.Core.Data;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Moq;
using NUnit.Framework;
using RichardSzalay.MockHttp;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace DepositUI.BLL.UnitTests
{
    [TestFixture]
    class DepositServiceUnitTests
    {
        private List<DepositModel> depositModelList;

        private List<DepositCalculation> depositCalculationList;

        private Mock<IAccessTokenProvider> tokenProviderMock;

        [SetUp]
        public void SetUp()
        {
            this.tokenProviderMock = new Mock<IAccessTokenProvider>();
            var tokenResult = new AccessTokenResult(AccessTokenResultStatus.Success, new AccessToken { Value = string.Empty }, string.Empty);
            this.tokenProviderMock.Setup(p => p.RequestAccessToken()).ReturnsAsync(tokenResult);

            this.depositCalculationList = new List<DepositCalculation>
            {
                new DepositCalculation { Month = 1 },
                new DepositCalculation { Month = 2 }
            };

            this.depositModelList = new List<DepositModel>
            {
                new DepositModel { Id = 0 },
                new DepositModel { Id = 1 }
            };
        }

        [Test]
        public async Task GetDepositsAsync_ExpectListDeposit_AsyncMethodCall()
        {
            var tokenProviderMock = this.tokenProviderMock;
            var httpClient = GetHttpClient(JsonSerializer.Serialize(this.depositModelList));
            var service = new DepositService(httpClient, tokenProviderMock.Object);

            var item = await service.GetDepositsAsync(0, 1);

            Assert.AreEqual(this.depositModelList[0].Id, item[0].Id);
            Assert.AreEqual(this.depositModelList[1].Id, item[1].Id);
            Assert.AreEqual(this.depositModelList.Count, item.Count);
        }

        [Test]
        public async Task GetDepositDetails_ExpectListDepositCalculation_AsyncMethodCall()
        {
            var httpClient = GetHttpClient(JsonSerializer.Serialize(this.depositCalculationList));
            var service = new DepositService(httpClient, tokenProviderMock.Object);

            var item = await service.GetDepositDetailsAsync(1);

            Assert.AreEqual(this.depositCalculationList[0].Month, item[0].Month);
            Assert.AreEqual(this.depositCalculationList[1].Month, item[1].Month);
            Assert.AreEqual(this.depositCalculationList.Count, item.Count);
        }

        [Test]
        public async Task CalculateDepositAsync_ExpectDepositCalculation_AsyncMethodCall()
        {
            var tokenProviderMock = this.tokenProviderMock;
            var httpClient = GetHttpClient(JsonSerializer.Serialize(this.depositCalculationList));
            var service = new DepositService(httpClient, tokenProviderMock.Object);

            var item = await service.CalculateDepositAsync(new DepositModel());

            Assert.AreEqual(this.depositCalculationList[0].Month, item[0].Month);
            Assert.AreEqual(this.depositCalculationList[1].Month, item[1].Month);
            Assert.AreEqual(this.depositCalculationList.Count, item.Count);
        }

        [Test]
        public async Task GetDepositCSVAsync_ExpectString_AsyncMethodCall()
        {
            string model = "TestString";
            var tokenProviderMock = this.tokenProviderMock;
            var httpClient = GetHttpClient(model);
            var service = new DepositService(httpClient, tokenProviderMock.Object);

            var item = await service.GetDepositCSVAsync(0);

            Assert.AreEqual(model, item);
        }

        private HttpClient GetHttpClient(string responseContent)
        {
            var handlerMock = new MockHttpMessageHandler();

            handlerMock.When("https://localhost:44320/api/*")
                .Respond("application/json", responseContent);

            return handlerMock.ToHttpClient();
        }
    }
}
