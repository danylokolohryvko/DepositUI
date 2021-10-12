using DepositUI.Core.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DepositUI.BLL.Interfaces
{
    public interface IDepositService
    {
        public Task<List<DepositModel>> GetDepositsAsync(int startIndex, int count);

        public Task<List<DepositCalc>> GetDepositDetailsAsync(int depositId);

        public Task<List<DepositCalc>> CalculateDepositAsync(DepositModel deposit);

        public Task<string> GetDepositCSV(int depositId);
    }
}
