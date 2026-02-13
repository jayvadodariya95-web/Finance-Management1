using FinanceManagement.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceManagement.Domain.Entities
{
    public class EmployeeDocument : BaseEntity
    {
        public int EmployeeId { get; set; }
        public int DocId { get; set; }
        //Navigation
        public Employee Employee { get; set; } = null!;
        public Documents Documents { get; set; } = null!;
    }
}
