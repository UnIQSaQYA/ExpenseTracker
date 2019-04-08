using ExpensTracker.DAL.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpensTracker.DAL
{
    public class ExpenseRepository : BaseRepository<ExpenseTrackerEntities, Expenses>, IExpenseRepository
    {
        public IQueryable<p_get_totalamount_by_date_Result> GetTotalAmountByDate(int userId)
        {
            return Context.p_get_totalamount_by_date(userId).AsQueryable();
        }

        public IQueryable<p_get_most_expensed_category_Result> GetMostExpensedCategory(int userId)
        {
            return Context.p_get_most_expensed_category(userId).AsQueryable();
        }
    }
}
