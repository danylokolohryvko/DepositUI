using DepositUI.BLL.Interfaces;
using DepositUI.Core.Data;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DepositUI.Pages
{
    public partial class Calculate
    {
        private List<DepositCalc> depositDetails;
        private DepositModel deposit = new DepositModel();

        [Inject]
        private IDepositService DepositService { get; set; }

        private async Task LoadDepositCalc()
        {
            depositDetails = await this.DepositService.CalculateDepositAsync(deposit);
        }
    }
}
