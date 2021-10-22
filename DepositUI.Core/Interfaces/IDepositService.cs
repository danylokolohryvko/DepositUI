using DepositUI.Core.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DepositUI.Core.Interfaces
{
    public interface IDepositService
    {
        public Task<List<DepositModel>> GetDepositsAsync(int startIndex, int count);

        public Task<List<DepositCalculation>> GetDepositDetailsAsync(int depositId);

        public Task<List<DepositCalculation>> CalculateDepositAsync(DepositModel deposit);

        public Task<string> GetDepositCSVAsync(int depositId);
    }
}
