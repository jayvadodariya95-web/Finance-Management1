using FinanceManagement.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceManagement.Domain.Entities
{
    public class Category: BaseEntity
    {
        public string? CategoryName { get; set; }
        public bool IsRecurring { get; set; }

        public ICollection<MonthlyExpense>? MonthlyExpenses { get; set; }
    }
}
