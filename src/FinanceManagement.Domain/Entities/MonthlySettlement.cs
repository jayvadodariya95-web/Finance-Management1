using FinanceManagement.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceManagement.Domain.Entities
{
  public class MonthlySettlement : BaseEntity
    {

        public int PartnerId { get; set; }
        public bool IsSettled { get; set; } = false;
        public DateTime? SettledDate { get; set; }
        public decimal? TotalExpenses { get; set; }
        public decimal? GrossProfit { get; set; }
        public decimal? NetProfit { get; set; }
        public Partner Partner { get; set; } = null!;
    }

}
