using FinanceManagement.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceManagement.Domain.Entities
{
    public class Asset : BaseEntity
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public decimal Amount { get; set; }
        public DateTime Purchase_Date { get; set; }
        public ICollection<MonthlyExpense>? MonthlyExpenses { get; set; } 

    }
}
