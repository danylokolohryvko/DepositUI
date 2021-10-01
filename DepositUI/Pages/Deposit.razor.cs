using AutoMapper;
using DepositUI.BLL.DTOs;
using DepositUI.BLL.Interfaces;
using DepositUI.Data;
using DepositUI.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DepositUI.Pages
{
    public partial class Deposit
    {
        private readonly IDepositService depositService;
        private readonly IMapper mapper;

        private const int columnNumber = 4;
        private ModeType mode = ModeType.Main;

        private List<DepositCalc> depositDetails;
        private List<RequestError> requestErrors;

        private string AmountStr { get { return amountStr; } set { amountStr = NumberInputCheck(value); } }
        private string amountStr;

        private string TermStr { get { return termStr; } set { termStr = NumberInputCheck(value); } }
        private string termStr;

        private string PercentStr { get { return percentStr; } set { percentStr = NumberInputCheck(value); } }
        private string percentStr;

        private bool getDepositError;

        private List<DepositModel> deposits;
        private int nextdeposit = 0;

        private int depositId;

        public Deposit(IDepositService depositService, IMapper mapper)
        {
            this.depositService = depositService;
            this.mapper = mapper;
        }

        private void Main()
        {
            depositDetails = null;
            mode = ModeType.Main;
        }

        private async Task History()
        {
            var depositsDTO = await this.depositService.GetDepositsAsync(0, 16);
            deposits = this.mapper.Map<List<DepositModel>>(depositsDTO);
            nextdeposit = deposits.Count;
            mode = ModeType.History;
        }

        private async Task LoadMore()
        {
            var loadedDTO = await this.depositService.GetDepositsAsync(nextdeposit, 16);
            var loaded = this.mapper.Map<List<DepositModel>>(loadedDTO);
            deposits.AddRange(loaded);
            nextdeposit += loaded.Count;
        }

        private async Task Details(int depositId)
        {
            var loadedDTO = await this.depositService.GetDepositDetailsAsync(depositId);

            if (loadedDTO == null)
            {
                return;
            }
            depositDetails = mapper.Map<List<DepositCalc>>(loadedDTO);
            this.depositId = depositId;
            mode = ModeType.Details;
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

            DepositDTO deposit = new DepositDTO()
            {
                Amount = amount,
                Percent = persent,
                Term = term
            };

            var depositDetailsDTO = await this.depositService.CalculateDepositAsync(deposit);
            depositDetails = this.mapper.Map<List<DepositCalc>>(depositDetailsDTO);
        }

        private string NumberInputCheck(string input)
        {
            string res = string.Join("", input.Where(c => char.IsDigit(c) || c == '.' || c == ','));
            return res;
        }
    }
}
