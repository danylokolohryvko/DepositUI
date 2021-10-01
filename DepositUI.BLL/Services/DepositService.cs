using DepositUI.BLL.DTOs;
using DepositUI.BLL.HttpModels;
using DepositUI.BLL.Interfaces;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace DepositUI.BLL.Services
{
    public class DepositService : IDepositService
    {
        private readonly string depositsUrl;
        private readonly string depositCalculationUrl;
        private readonly string calculateDepositUrl;
        private readonly IHttpClientFactory clientFactory;


        public DepositService(IHttpClientFactory clientFactory, IConfiguration configuration)
        {
            this.clientFactory = clientFactory;
            depositsUrl = configuration.GetSection("ApiURLs").GetSection("GetDeposits").Value;
            depositCalculationUrl = configuration.GetSection("ApiURLs").GetSection("GetDepositCalculations").Value;
            calculateDepositUrl = configuration.GetSection("ApiURLs").GetSection("CalculateDeposit").Value;
        } 

        public async Task<List<DepositDTO>> GetDepositsAsync(int startIndex, int count)
        {
            var model = new GetDepositsModel
            {
                StartIndex = startIndex,
                Count = count
            };
            var response = await this.SendRequestAsync(HttpMethod.Get, depositsUrl, JsonContent.Create<GetDepositsModel>(model));
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<List<DepositDTO>>();
            }
            return null;
        }

        public async Task<List<DepositCalcDTO>> GetDepositDetailsAsync(int depositId)
        {
            var model = new GetDepositDetails
            {
                DepositId = depositId
            };
            var response = await this.SendRequestAsync(HttpMethod.Get, depositCalculationUrl, JsonContent.Create<GetDepositDetails>(model));
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<List<DepositCalcDTO>>();
            }
            return null;
        }

        public async Task<List<DepositCalcDTO>> CalculateDepositAsync(DepositDTO deposit)
        {
            var response = await this.SendRequestAsync(HttpMethod.Get, calculateDepositUrl, JsonContent.Create<DepositDTO>(deposit));
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<List<DepositCalcDTO>>();
            }
            return null;
        }

        private async Task<HttpResponseMessage> SendRequestAsync(HttpMethod method,string url, HttpContent content)
        {
            var request = new HttpRequestMessage(method, url);
            request.Content = content;
            var client = this.clientFactory.CreateClient();
            return await client.SendAsync(request);
        }
    }
}
