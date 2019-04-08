using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.Model
{
    public class Role
    {
        public int Id { get; set; }
        public string RoleName { get; set; }
        public string Remarks { get; set; }
        public DateTime CreatedAt { get; set; }
        public virtual ICollection<User> Users { get; set; }
    }
}
