using ExpenseTracker.BAL;
using ExpenseTracker.Model;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ExpenseTracker.Controllers
{
    [Authorize]
    public class ExpenseController : BaseController
    {
        ExpenseService _expenseService = new ExpenseService();
        CategoryService _categoryService = new CategoryService();
        // GET: Expense
        public ActionResult Index(string searchString, DateTime? startDate, DateTime? endDate, int page = 1)
        {
            ExpenseTrackerIdentity ident = User.Identity as ExpenseTrackerIdentity;
            var expense = _expenseService.GetExpenses(ident.UserId, searchString, startDate, endDate);
            return Request.IsAjaxRequest()
             ? (ActionResult)PartialView("_list", expense.ToPagedList(page, pageSize))
             : View(expense.ToPagedList(page, pageSize));
        }

        [HttpGet]
        public ActionResult Create()
        {
            LoadCategory();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Expense expense)
        {
            LoadCategory();
            if (ModelState.IsValid)
            {
                ExpenseTrackerIdentity ident = User.Identity as ExpenseTrackerIdentity;

                expense.CreatedBy = ident.UserId;
                expense.CreatedAt = DateTime.Now;
                var data = _expenseService.Create(expense);
                ShowStatus(data.StatusCode, data.Status);
                if (data.StatusCode == 200)
                    return RedirectToAction("Index", "Expense");
            }
            return View();
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            //There user id is used to get the expense, to prevent the user from getting some one else data
            //by manipulating the parameters
            ExpenseTrackerIdentity ident = User.Identity as ExpenseTrackerIdentity;
            LoadCategory();
            var expense = _expenseService.GetExpenseById(id, ident.UserId);
            if(expense == null)
            {
                ShowStatus(400, "Expense with id " + id.ToString() + " does not exist.");
                return RedirectToAction("Index", "Expense");
            }
            return View(expense);
        }

        [HttpPost]
        public ActionResult Edit(Expense expense)
        {
            LoadCategory();
            if (ModelState.IsValid)
            {
                var data = _expenseService.Update(expense);
                ShowStatus(data.StatusCode, data.Status);
                if (data.StatusCode == 200)
                    return RedirectToAction("Index", "Expense");
            }
            return View(expense);
        }
        [HttpGet]
        public ActionResult DeleteConfirmation(int id)
        {
            ExpenseTrackerIdentity ident = User.Identity as ExpenseTrackerIdentity;
            var expense = _expenseService.GetExpenseById(id, ident.UserId);
            if (expense == null)
            {
                ShowStatus(400, "Expense with id " + id.ToString() + " does not exist.");
                return RedirectToAction("Index", "Expense");
            }

            return PartialView("_deleteConfirmation", expense);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(Expense expense)
        {
            var data = _expenseService.Delete(expense);
            ShowStatus(data.StatusCode, data.Status);
            return RedirectToAction("Index", "Expense");
        }
        #region private helper class
        private void LoadCategory()
        {
            ExpenseTrackerIdentity ident = User.Identity as ExpenseTrackerIdentity;
            var category = _categoryService.GetCategories(ident.UserId, null, null);
            ViewBag.categories = category;
        }
        #endregion
    }
}