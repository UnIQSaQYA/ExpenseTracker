using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.Model
{
    public class User
    {
        public string Name { get; set; }
        public int Id { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string Occupation { get; set; }
        public string Password { get; set; }
        public bool Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public string RoleName { get; set; }
        public int RoleId { get; set; }
        public string ConfirmPassword { get; set; }
    }
}
