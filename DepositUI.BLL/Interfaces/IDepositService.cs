using DepositUI.BLL.DTOs;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DepositUI.BLL.Interfaces
{
    public interface IDepositService
    {
        public Task<List<DepositDTO>> GetDepositsAsync(int startIndex, int count);

        public Task<List<DepositCalcDTO>> GetDepositDetailsAsync(int depositId);

        public Task<List<DepositCalcDTO>> CalculateDepositAsync(DepositDTO deposit);
    }
}
