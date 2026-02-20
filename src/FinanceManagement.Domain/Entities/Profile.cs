using FinanceManagement.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceManagement.Domain.Entities
{
    public class Profile : BaseEntity
    {
        public int UserId { get; set; }
        public bool IsPaid { get; set; } = false;
        public int? Amount { get; set; }
        public User User { get; set; } = null!;
        public ICollection<Project> Projects { get; set; } = new List<Project>();
    }

}
