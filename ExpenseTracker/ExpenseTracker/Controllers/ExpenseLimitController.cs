using ExpenseTracker.BAL;
using ExpenseTracker.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ExpenseTracker.Controllers
{
    public class ExpenseLimitController : BaseController
    {
        ExpenseLimitService _expenseLimitService = new ExpenseLimitService();
        // GET: ExpenseLimit
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(ExpenseLimit expense)
        {
            if (ModelState.IsValid)
            {
                ExpenseTrackerIdentity ident = User.Identity as ExpenseTrackerIdentity;
                expense.Status = true;
                expense.CreatedBy = ident.UserId;
                var msg = _expenseLimitService.Create(expense);
                ShowStatus(msg.StatusCode, msg.Status);
                if (msg.StatusCode == 200)
                    return RedirectToAction("Index", "Dashboard");
            }
            return View();
        }

        [HttpGet]
        public ActionResult Edit()
        {
            ExpenseTrackerIdentity ident = User.Identity as ExpenseTrackerIdentity;
            var expenseLimit = _expenseLimitService.GetExpenseByUser(ident.UserId);
            if (expenseLimit == null)
            {
                ShowStatus(400, "Expense Limit for user " + ident.UserName + " does not exist.");
                return RedirectToAction("Create", "ExpenseLimit");
            }
            return View(expenseLimit);
        }

        [HttpPost]
        public ActionResult Edit(ExpenseLimit expenseLimit)
        {
            if (ModelState.IsValid)
            {
                ExpenseTrackerIdentity ident = User.Identity as ExpenseTrackerIdentity;
                expenseLimit.CreatedBy = ident.UserId;
                var data = _expenseLimitService.Update(expenseLimit);
                ShowStatus(data.StatusCode, data.Status);
                if (data.StatusCode == 200)
                    return RedirectToAction("Index", "Dashboard");
            }

            return View(expenseLimit);
        }
    }
}