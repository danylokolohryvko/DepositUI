using DepositUI.Core.Interfaces;
using DepositUI.Core.Data;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DepositUI.Pages
{
    public partial class Calculate
    {
        public List<DepositCalculation> DepositDetails { get; private set; }
        public DepositModel Deposit { get; private set; }

        [Inject]
        private IDepositService DepositService { get; set; }

        public Calculate()
        {
            Deposit = new DepositModel();
        }

        private async Task LoadDepositCalc()
        {
            DepositDetails = await this.DepositService.CalculateDepositAsync(Deposit);
        }
    }
}
