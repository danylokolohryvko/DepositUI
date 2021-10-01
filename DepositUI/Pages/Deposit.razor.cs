using AutoMapper;
using DepositUI.BLL.DTOs;
using DepositUI.BLL.Interfaces;
using DepositUI.Data;
using DepositUI.Enums;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DepositUI.Pages
{
    public partial class Deposit
    {
        [Inject]
        private IDepositService depositService { get; set; }
        [Inject]
        private IMapper mapper { get; set; }

        //General
        private ModeType mode = ModeType.Main;

        //Main and Details
        private List<DepositCalc> depositDetails;
        private DepositModel deposit = new DepositModel();
        private int depositId;

        //History
        private const int columnNumber = 4;
        private int nextdeposit = 0;
        private List<DepositModel> deposits;

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
            var depositDTO = this.mapper.Map<DepositDTO>(deposit);
            var depositDetailsDTO = await this.depositService.CalculateDepositAsync(depositDTO);
            depositDetails = this.mapper.Map<List<DepositCalc>>(depositDetailsDTO);
        }
    }
}
