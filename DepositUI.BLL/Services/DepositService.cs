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
        private const string depositsUrl = "https://localhost:44320/api/deposit";
        private const string depositCalculationUrl = "https://localhost:44320/api/depositcalculation";
        private const string calculateDepositUrl = "https://localhost:44320/api/calculate";
        private readonly HttpClient client;


        public DepositService(HttpClient client, IConfiguration configuration)
        {
            this.client = client;
        } 

        public async Task<List<DepositDTO>> GetDepositsAsync(int startIndex, int count)
        {
            var model = new GetDepositsModel
            {
                StartIndex = startIndex,
                Count = count
            };
            var response = await this.SendRequestAsync(HttpMethod.Post, depositsUrl, JsonContent.Create<GetDepositsModel>(model));
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
            var response = await this.SendRequestAsync(HttpMethod.Post, depositCalculationUrl, JsonContent.Create<GetDepositDetails>(model));
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<List<DepositCalcDTO>>();
            }
            return null;
        }

        public async Task<List<DepositCalcDTO>> CalculateDepositAsync(DepositDTO deposit)
        {
            var response = await this.SendRequestAsync(HttpMethod.Post, calculateDepositUrl, JsonContent.Create<DepositDTO>(deposit));
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
            return await client.SendAsync(request);
        }
    }
}
