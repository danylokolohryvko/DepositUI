using DepositUI.Core.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DepositUI.Core.Data
{
    public class DepositModel
    {
        public int Id { get; set; }

        [Required]
        public double? Amount { get; set; }

        [Required]
        public int? Term { get; set; }

        [Required]
        public double? Percent { get; set; }

        public DateTime Date { get; set; }

        public CalculationType CalculationType { get; set; }
    }
}
