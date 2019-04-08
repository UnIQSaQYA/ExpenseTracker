using ExpenseTracker.BAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ExpenseTracker.Controllers
{
    [Authorize]
    public class ReportController : Controller
    {
        ExpenseService _expenseService = new ExpenseService();
        CategoryService _categoryService = new CategoryService();
        // GET: Report
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ExpenseReport(DateTime? startDate, DateTime? endDate, int? categoryId)
        {
            LoadCategory();
            //Gets user session data
            ExpenseTrackerIdentity ident = User.Identity as ExpenseTrackerIdentity;

            var expense = _expenseService.GetExpenseReport(ident.UserId, startDate, endDate, categoryId);
            return View(expense);
        }

        #region private helpers
        private void LoadCategory()
        {
            ExpenseTrackerIdentity ident = User.Identity as ExpenseTrackerIdentity;
            var category = _categoryService.GetCategories(ident.UserId, null, null);
            ViewBag.categories = category;
        }
        #endregion
    }
}