using ExpensTracker.DAL.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpensTracker.DAL
{
    public interface IExpenseRepository : IBaseRepository<Expenses>
    {
        IQueryable<p_get_totalamount_by_date_Result> GetTotalAmountByDate(int userId);
        IQueryable<p_get_most_expensed_category_Result> GetMostExpensedCategory(int userId);
    }
}
