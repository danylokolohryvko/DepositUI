using System;
using System.Collections.Generic;
using System.Text;

namespace DepositUI.Core.Configuration
{
    public class Urls
    {
        public static string GetDeposits { get { return "https://localhost:44320/api/deposit"; } }
        public static string GetDepositCalculations { get { return "https://localhost:44320/api/depositcalculation"; } }
        public static string GetDepositCSV { get { return "https://localhost:44320/api/depositcalculation/csv"; } }

        public static string CalculateDeposit { get { return "https://localhost:44320/api/calculate"; } }
    }
}
