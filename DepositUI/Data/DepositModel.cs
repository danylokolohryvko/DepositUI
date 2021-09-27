using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DepositUI.Data
{
    public class DepositModel
    {
        public double? Amount { get; set; }

        public int? Term { get; set; }

        public double? Percent { get; set; }
    }
}
