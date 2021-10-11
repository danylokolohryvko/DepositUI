using System;
using System.Collections.Generic;
using System.Text;

namespace DepositUI.Core.Configuration
{
    public class Urls
    {
        public static string Deposits { get { return "https://localhost:44320/api/deposit"; } }
        public static string DepositCalculations { get { return "https://localhost:44320/api/depositcalculation"; } }
        public static string DepostCSV { get { return "https://localhost:44320/api/depositcalculation/csv"; } }

        public static string CalculateDeposit { get { return "https://localhost:44320/api/calculate"; } }
    }
}
