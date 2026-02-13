using FinanceManagement.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceManagement.Domain.Entities
{
    class Documents : BaseEntity
    {
        //Foreign key
        public int DocType_id { get; set; }

        public string? Link { get; set; }

        //Navigation property
        public DocType? DocType { get; set; }
    }
}
