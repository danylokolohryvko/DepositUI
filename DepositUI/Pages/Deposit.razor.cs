using DepositUI.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace DepositUI.Pages
{
    public partial class Deposit
    {
        private string mode = "Main";

        private List<DepositCalc> depositDetails;
        private List<RequestError> requestErrors;

        private string AmountStr { get { return amountStr; } set { amountStr = NumberInputCheck(value); } }
        private string amountStr;

        private string TermStr { get { return termStr; } set { termStr = NumberInputCheck(value); } }
        private string termStr;

        private string PercentStr { get { return percentStr; } set { percentStr = NumberInputCheck(value); } }
        private string percentStr;

        private bool getDepositError;
        private HttpResponseMessage response = null;

        private List<DepositModel> deposits;
        private int nextdeposit = 0;

        private int depositId;

        private void Main()
        {
            depositDetails = null;
            mode = "Main";
        }

        private async Task History()
        {
            deposits = await GetDepositModels(0, 16);
            nextdeposit += deposits.Count;
            mode = "History";
        }

        private async Task Details(int depositId)
        {
            var model = new GetDepositDetails
            {
                DepositId = depositId
            };
            var request = new HttpRequestMessage(HttpMethod.Get, "https://localhost:44320/api/depositcalc");
            request.Content = JsonContent.Create<GetDepositDetails>(model);

            var client = ClientFactory.CreateClient();
            response = await client.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                depositDetails = await response.Content.ReadFromJsonAsync<List<DepositCalc>>();
            }
            this.depositId = depositId;
            mode = "Details";
        }

        private async Task LoadMore()
        {
            var loaded = await GetDepositModels(nextdeposit, 16);
            deposits.AddRange(loaded);
            nextdeposit += loaded.Count;
        }

        private async Task GetDepositCalc()
        {
            requestErrors = new List<RequestError>();
            double amount;
            double persent;
            int term;

            if (!Double.TryParse(NumberInputCheck(amountStr), out amount))
            {
                requestErrors.Add(new RequestError
                {
                    PropertyName = "Amount",
                    ErrorMessage = "Amount is not number"
                });
                getDepositError = true;
            }

            if (!Double.TryParse(NumberInputCheck(percentStr), out persent))
            {
                requestErrors.Add(new RequestError
                {
                    PropertyName = "Percent",
                    ErrorMessage = "Percent is not number"
                });
                getDepositError = true;
            }

            if (!Int32.TryParse(NumberInputCheck(termStr), out term))
            {
                requestErrors.Add(new RequestError
                {
                    PropertyName = "Term",
                    ErrorMessage = "Term is not number"
                });
                getDepositError = true;
            }

            DepositModel deposit = new DepositModel()
            {
                Amount = amount,
                Percent = persent,
                Term = term
            };

            var request = new HttpRequestMessage(HttpMethod.Get, "https://localhost:44320/api/calculate");
            request.Content = JsonContent.Create<DepositModel>(deposit);

            var client = ClientFactory.CreateClient();
            response = await client.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                depositDetails = await response.Content.ReadFromJsonAsync<List<DepositCalc>>();
            }
            else
            {
                requestErrors.AddRange(await response.Content.ReadFromJsonAsync<List<RequestError>>());
                getDepositError = true;
            }
        }

        private async Task<List<DepositModel>> GetDepositModels(int startIndex, int count)
        {
            var model = new GetDepositsModel
            {
                StartIndex = startIndex,
                Count = count
            };
            var request = new HttpRequestMessage(HttpMethod.Get, "https://localhost:44320/api/deposit");
            request.Content = JsonContent.Create<GetDepositsModel>(model);

            var client = ClientFactory.CreateClient();
            response = await client.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<List<DepositModel>>();
            }
            return null;
        }

        private string NumberInputCheck(string input)
        {
            string res = string.Join("", input.Where(c => char.IsDigit(c) || c == '.' || c == ','));
            return res;
        }
    }
}
