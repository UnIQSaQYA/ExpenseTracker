using ExpenseTracker.BAL;
using ExpenseTracker.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ExpenseTracker.Controllers
{
    [Authorize]
    public class DashboardController : BaseController
    {
        CategoryService _categoryService = new CategoryService();
        ExpenseService _expenseService = new ExpenseService();
        ExpenseLimitService _expenseLimitService = new ExpenseLimitService();
        // GET: Dashboard
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult LimitNotifier()
        {
            ExpenseTrackerIdentity ident = User.Identity as ExpenseTrackerIdentity;
            var model = new DashboardExpenseLimit();
            model.ExpenseLimit = _expenseLimitService.GetExpenseByUser(ident.UserId);
            model.Expenses = _expenseService.GetTodayExpense(ident.UserId); 
            return PartialView("_limitNotifier", model);
        }
        public ActionResult Counter()
        {
            ExpenseTrackerIdentity ident = User.Identity as ExpenseTrackerIdentity;
            DashboardCounter model = new DashboardCounter();
            model.ExpenseCount = _expenseService.ExpenseCount(ident.UserId);
            model.CategoryCount = _categoryService.CategoryCount(ident.UserId);
            model.TotalExpenseToday = _expenseService.GetTotalExpenseToday(ident.UserId);
            model.TotalExpense = _expenseService.GetTotalExpense(ident.UserId);
            return PartialView("_counter", model);
        }

        public ActionResult ExpenseChart()
        {
            ExpenseTrackerIdentity ident = User.Identity as ExpenseTrackerIdentity;
            var model = _expenseService.GetTotalAmountByDate(ident.UserId);
            var expensByCategory = _expenseService.GetMostExpensedCategory(ident.UserId);
            ViewBag.expense = model;
            ViewBag.expenseByCategory = expensByCategory;
            return PartialView("_expenseChart");
        }

        public ActionResult RecentCategory()
        {
            ExpenseTrackerIdentity ident = User.Identity as ExpenseTrackerIdentity;
            var category = _categoryService.GetCategories(ident.UserId, null, null).Take(5).ToList();
            return PartialView("_recentCategory", category);
        }

        public ActionResult RecentExpense()
        {
            ExpenseTrackerIdentity ident = User.Identity as ExpenseTrackerIdentity;
            var expense = _expenseService.GetExpenses(ident.UserId, null, null, null).Take(5).ToList();
            return PartialView("_recentExpense", expense);
        }
    }
}