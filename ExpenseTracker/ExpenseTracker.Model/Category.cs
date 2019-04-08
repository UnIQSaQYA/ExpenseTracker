using ExpensTracker.DAL.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.Model
{
    public class Category
    {

        public Category()
           : base()
        {
        }

        public Categories ToDalEntity()
        {
            return ToDalEntity(new Categories());
        }

        public Categories ToDalEntity(Categories category)
        {
            category.Id = this.Id;
            category.Name = this.Name;
            category.Remarks = this.Remarks;
            category.Status = this.Status;
            category.CreatedAt = (!this.CreatedAt.HasValue) ? category.CreatedAt : this.CreatedAt.Value;
            category.CreatedBy = (!this.CreatedBy.HasValue) ? category.CreatedBy : this.CreatedBy.Value;
            category.ModifiedAt = (!this.ModifiedAt.HasValue) ? category.ModifiedAt : this.ModifiedAt.Value;
            return category;
        }
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Remarks { get; set; }
        public int Status { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? ModifiedAt { get; set; }
    }
}
