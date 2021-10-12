using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DepositUI.Core.Data
{
    public class DepositModel
    {
        public int Id { get; set; }

        public double? Amount { get; set; }

        public int? Term { get; set; }

        public double? Percent { get; set; }

        public DateTime Date { get; set; }
    }
}
