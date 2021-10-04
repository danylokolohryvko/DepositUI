using AutoMapper;
using DepositUI.BLL.DTOs;
using DepositUI.BLL.Interfaces;
using DepositUI.Data;
using DepositUI.Enums;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DepositUI.Pages
{
    public partial class Deposit
    {
        private const string getDepostCSV = "https://localhost:44320/api/depositcalculation/csv";

        [Inject]
        private IDepositService DepositService { get; set; }
        [Inject]
        private IMapper Mapper { get; set; }
        [Inject]
        private IConfiguration Configuration { get; set; }

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
            var depositsDTO = await this.DepositService.GetDepositsAsync(0, 16);
            deposits = this.Mapper.Map<List<DepositModel>>(depositsDTO);
            nextdeposit = deposits.Count;
            mode = ModeType.History;
        }

        private async Task LoadMore()
        {
            var loadedDTO = await this.DepositService.GetDepositsAsync(nextdeposit, 16);
            var loaded = this.Mapper.Map<List<DepositModel>>(loadedDTO);
            deposits.AddRange(loaded);
            nextdeposit += loaded.Count;
        }

        private async Task Details(int depositId)
        {
            var loadedDTO = await this.DepositService.GetDepositDetailsAsync(depositId);

            if (loadedDTO == null)
            {
                return;
            }
            depositDetails = Mapper.Map<List<DepositCalc>>(loadedDTO);
            this.depositId = depositId;
            mode = ModeType.Details;
        }

        private async Task GetDepositCalc()
        {
            var depositDTO = this.Mapper.Map<DepositDTO>(deposit);
            var depositDetailsDTO = await this.DepositService.CalculateDepositAsync(depositDTO);
            depositDetails = this.Mapper.Map<List<DepositCalc>>(depositDetailsDTO);
        }
    }
}
