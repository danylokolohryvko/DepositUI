using DepositUI.Core.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace DepositUI.UnitTests.Utility
{
    class ModelsProvider
    {
        public static List<DepositCalculation> DepositCalculationList
        {
            get
            {
                return new List<DepositCalculation>
                {
                    new DepositCalculation { Month = 1, PercentAdded = 1, TotalAmount = 11 },
                    new DepositCalculation { Month = 2, PercentAdded = 1, TotalAmount = 12 },
                };
            }
        }

        public static List<DepositModel> DepositModelList
        {
            get
            {
                return new List<DepositModel>
                {
                    new DepositModel { Percent = 1, Term = 10, Amount = 100, Date = DateTime.MinValue },
                    new DepositModel { Percent = 2, Term = 20, Amount = 200, Date = DateTime.MaxValue },
                };
            }
        }
    }
}
