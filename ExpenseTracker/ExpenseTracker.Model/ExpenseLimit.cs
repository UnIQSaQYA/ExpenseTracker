using ExpensTracker.DAL.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.Model
{
    public class ExpenseLimit
    {
        public ExpenseLimit()
          : base()
        {
        }
        public ExpenseLimits ToDalEntity()
        {
            return ToDalEntity(new ExpenseLimits());
        }

        public ExpenseLimits ToDalEntity(ExpenseLimits expenseLimit)
        {
            expenseLimit.Id = (this.Id == 0) ? expenseLimit.Id: this.Id;
            expenseLimit.LimitAmount = this.LimitAmount;
            expenseLimit.Status = this.Status;
            expenseLimit.CreatedBy = this.CreatedBy;

            return expenseLimit;
        }
        public int Id { get; set; }
        [Required]
        public Decimal LimitAmount { get; set; }
        public int CreatedBy { get; set; }
        public bool Status { get; set; }
    }
}
