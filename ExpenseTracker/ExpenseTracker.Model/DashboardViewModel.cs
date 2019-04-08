using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.Model
{
    public class DashboardViewModel
    {

    }

    public class DashboardCounter
    {
        public int ExpenseCount { get; set; }
        public int CategoryCount { get; set; }
        public decimal? TotalExpenseToday { get; set; }
        public decimal? TotalExpense { get; set; }
    }

    public class ExpenseChart
    {
        public Nullable<decimal> totalAmount { get; set; }
        public string date { get; set; }
    }

    public class ExpenseChartByCategory
    {
        public int? Count { get; set; }
        public string CategoryName { get; set; }
    }

    public class DashboardExpenseLimit
    {

        public ExpenseLimit ExpenseLimit { get; set; }

        public List<Expense> Expenses { get; set; }
    }
}
