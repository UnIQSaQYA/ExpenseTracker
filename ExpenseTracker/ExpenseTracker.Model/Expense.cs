using ExpensTracker.DAL.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.Model
{
    public class Expense
    {
        public Expense()
           : base()
        {
        }

        public Expenses ToDalEntity()
        {
            return ToDalEntity(new Expenses());
        }

        public Expenses ToDalEntity(Expenses expense)
        {
            expense.Id = this.Id;
            expense.Amount = this.Amount;
            expense.CategoryId = this.CategoryId;
            expense.CreatedAt = (!this.CreatedAt.HasValue) ? expense.CreatedAt : this.CreatedAt.Value;
            expense.CreatedBy = (!this.CreatedBy.HasValue) ? expense.CreatedBy : this.CreatedBy.Value;
            expense.ModifiedAt = (!this.ModifiedAt.HasValue) ? expense.ModifiedAt : this.ModifiedAt.Value;
            expense.Date = Convert.ToDateTime(this.ViewDate);
            expense.Description = this.Description;
            expense.Item = this.Item;

            return expense;
        }
        public int Id { get; set; }
        [Required]
        public string ViewDate { get; set; }
        public DateTime Date { get; set; }
        [Required]
        public Decimal Amount { get; set; }
        [Required]
        [MaxLength(100)]
        public string Item { get; set; }
        [MaxLength(150)]
        public string Description { get; set; }
        public int? CreatedBy { get; set; }
        public int CategoryId { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? ModifiedAt { get; set; }
        
        public string CategoryName { get; set; }
    }
}
