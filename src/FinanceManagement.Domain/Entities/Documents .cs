using FinanceManagement.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceManagement.Domain.Entities
{
    public class Documents : BaseEntity
    {
        public int DocType_Id { get; set; }
        public string? Link { get; set; }
        //Navigation property
        public DocType? DocType { get; set; }

        public ICollection<EmployeeDocument> EmployeeDocuments { get; set; } = new List<EmployeeDocument>();

    }
}
