using AutoMapper;
using DepositUI.BLL.DTOs;
using DepositUI.BLL.Interfaces;
using DepositUI.Data;
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
        [Inject]
        private IMapper Mapper { get; set; }

        private async Task GetDepositCalc()
        {
            var depositDTO = this.Mapper.Map<DepositDTO>(deposit);
            var depositDetailsDTO = await this.DepositService.CalculateDepositAsync(depositDTO);
            depositDetails = this.Mapper.Map<List<DepositCalc>>(depositDetailsDTO);
        }
    }
}
