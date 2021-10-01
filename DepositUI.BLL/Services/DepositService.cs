
using DepositUI.BLL.DTOs;
using DepositUI.BLL.HttpModels;
using DepositUI.BLL.Interfaces;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace DepositUI.BLL.Services
{
    public class DepositService : IDepositService
    {
        private readonly IHttpClientFactory clientFactory;

        public DepositService(IHttpClientFactory clientFactory)
        {
            this.clientFactory = clientFactory;
        } 

        public async Task<List<DepositDTO>> GetDepositsAsync(int startIndex, int count)
        {
            var model = new GetDepositsModel
            {
                StartIndex = startIndex,
                Count = count
            };
            var response = await this.SendRequestAsync(HttpMethod.Get, "https://localhost:44320/api/deposit", JsonContent.Create<GetDepositsModel>(model));
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
            var response = await this.SendRequestAsync(HttpMethod.Get, "https://localhost:44320/api/depositcalc", JsonContent.Create<GetDepositDetails>(model));
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<List<DepositCalcDTO>>();
            }
            return null;
        }

        public async Task<List<DepositCalcDTO>> CalculateDepositAsync(DepositDTO deposit)
        {
            var response = await this.SendRequestAsync(HttpMethod.Get, "https://localhost:44320/api/calculate", JsonContent.Create<DepositDTO>(deposit));
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
