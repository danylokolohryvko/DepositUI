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
        private readonly string depositsUrl = "https://localhost:44320/api/deposit";
        private readonly string depositCalculationUrl = "https://localhost:44320/api/depositcalculation";
        private readonly string calculateDepositUrl = "https://localhost:44320/api/calculate";
        private readonly HttpClient client;


        public DepositService(HttpClient client, IConfiguration config)
        {
            this.client = client;
            this.depositsUrl = config.GetSection("ApiURLs").GetSection("GetDeposits").Value;
            this.depositCalculationUrl = config.GetSection("ApiURLs").GetSection("GetDepositCalculations").Value;
            this.calculateDepositUrl = config.GetSection("ApiURLs").GetSection("CalculateDeposit").Value;
        } 

        public async Task<List<DepositDTO>> GetDepositsAsync(int startIndex, int count)
        {
            var url = $"{depositsUrl}?StartIndex={startIndex}&Count={count}";
            var response = await this.SendRequestAsync(HttpMethod.Get, url);
            
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<List<DepositDTO>>();
            }
            return null;
        }

        public async Task<List<DepositCalcDTO>> GetDepositDetailsAsync(int depositId)
        {
            var url = $"{depositCalculationUrl}?DepositId={depositId}";
            var response = await this.SendRequestAsync(HttpMethod.Get, url);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<List<DepositCalcDTO>>();
            }
            return null;
        }

        public async Task<List<DepositCalcDTO>> CalculateDepositAsync(DepositDTO deposit)
        {
            var url = $"{calculateDepositUrl}?Amount={deposit.Amount}&Term={deposit.Term}&Percent={deposit.Percent}";
            var response = await this.SendRequestAsync(HttpMethod.Get, url);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<List<DepositCalcDTO>>();
            }
            return null;
        }

        private async Task<HttpResponseMessage> SendRequestAsync(HttpMethod method,string url)
        {
            var request = new HttpRequestMessage(method, url);
            return await client.SendAsync(request);
        }
    }
}
