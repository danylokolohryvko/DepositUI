﻿using DepositUI.BLL.Interfaces;
using DepositUI.Core.Data;
using DepositUI.Core.Configuration;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace DepositUI.BLL.Services
{
    public class DepositService : IDepositService
    {
        private readonly string depositsUrl = Urls.Deposits;
        private readonly string depositCalculationUrl = Urls.DepositCalculations;
        private readonly string calculateDepositUrl = Urls.CalculateDeposit;
        private readonly string DepositCSVUrl = Urls.DepostCSV;
        private readonly HttpClient client;
        private readonly IAccessTokenProvider tokenProvider;


        public DepositService(HttpClient client, IAccessTokenProvider tokenProvider)
        {
            this.client = client;
            this.tokenProvider = tokenProvider;
        } 

        public async Task<List<DepositModel>> GetDepositsAsync(int startIndex, int count)
        {
            var url = $"{depositsUrl}?StartIndex={startIndex}&Count={count}";
            var response = await this.SendRequestAsync(HttpMethod.Get, url);
            
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<List<DepositModel>>();
            }

            return null;
        }

        public async Task<List<DepositCalc>> GetDepositDetailsAsync(int depositId)
        {
            var url = $"{depositCalculationUrl}?DepositId={depositId}";
            var response = await this.SendRequestAsync(HttpMethod.Get, url);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<List<DepositCalc>>();
            }

            return null;
        }

        public async Task<List<DepositCalc>> CalculateDepositAsync(DepositModel deposit)
        {
            var url = $"{calculateDepositUrl}?Amount={deposit.Amount}&Term={deposit.Term}&Percent={deposit.Percent}";
            var response = await this.SendRequestAsync(HttpMethod.Get, url);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<List<DepositCalc>>();
            }

            return null;
        }

        public async Task<string> GetDepositCSV(int depositId)
        {
            var url = $"{DepositCSVUrl}?depositId={depositId}";
            var response = await this.SendRequestAsync(HttpMethod.Get, url);

            if(response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStringAsync();
            }

            return null;
        }

        private async Task<HttpResponseMessage> SendRequestAsync(HttpMethod method,string url)
        {
            var tokenresult = await tokenProvider.RequestAccessToken();
            tokenresult.TryGetToken(out AccessToken token);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.Value);
            var request = new HttpRequestMessage(method, url);

            return await client.SendAsync(request);
        }
    }
}
