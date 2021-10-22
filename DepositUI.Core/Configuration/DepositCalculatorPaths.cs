namespace DepositUI.Core.Configuration
{
    public class DepositCalculatorPaths
    {
        private const string basePath = "https://localhost:44320/api";

        public static string GetDeposits { get { return $"{basePath}/deposit"; } }
        public static string GetDepositCalculations { get { return $"{basePath}/depositcalculation"; } }
        public static string GetDepositCSV { get { return $"{basePath}/depositcalculation/csv"; } }

        public static string CalculateDeposit { get { return $"{basePath}/calculate"; } }
    }
}
