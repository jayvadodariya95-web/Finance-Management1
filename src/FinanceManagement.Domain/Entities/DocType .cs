using FinanceManagement.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceManagement.Domain.Entities
{
    public class DocType : BaseEntity
    {
        public string? TypeName { get; set; }

        public ICollection<Documents>? Documents { get; set; }
    }
}
