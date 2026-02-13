using FinanceManagement.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceManagement.Domain.Entities
{
    public class Revenue : BaseEntity
    {
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public int MyProperty { get; set; }

        public bool Revenue_From { get; set; }
        public string? Notes { get; set; }
        //CONSTRAINT FK_Revenue_Partner
        //FOREIGN KEY(Partner_id) REFERENCES Partner(Id)
    }
}
