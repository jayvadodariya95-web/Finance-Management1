using FinanceManagement.Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceManagement.Domain.Entities
{
    public class Revenue:BaseEntity
    {
        public int Partner_id { get; set; }
        public int? Project_id { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }

        public bool Revenue_From { get; set; } = true;
        public string? Notes { get; set; }

        public Project? Project { get; set; }
        // Navigation properties
        public Partner? Partner { get; set; }

    }
}
