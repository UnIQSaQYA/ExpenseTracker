using ExpenseTracker.Model;
using ExpensTracker.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.BAL
{
    public class ExpenseService
    {
        private readonly IExpenseRepository _expenseRepository;
        public ExpenseService()
        {
            _expenseRepository = new ExpenseRepository();
        }

        public List<Expense> GetExpenses(int userId, string searchString, DateTime? startDate, DateTime? endDate)
        {
            //What the query does??
            //Selects the expenses based on current user session
            //searches the database based on item and category if search string is not null

            var expense = _expenseRepository.FindBy(x => x.CreatedBy == userId && (searchString == null || (x.Item + x.Categories.Name).Contains(searchString)) && (startDate == null || x.Date >= startDate) && (endDate == null || x.Date <= endDate))
                .OrderByDescending(x => x.Id).Select(x => new Expense
                {
                    Id = x.Id,
                    Amount = x.Amount,
                    CategoryId = x.CategoryId,
                    CreatedAt = x.CreatedAt,
                    CreatedBy = x.CreatedBy,
                    Date = x.Date,
                    Description = x.Description,
                    Item = x.Item,
                    ModifiedAt = x.ModifiedAt,
                    CategoryName = x.Categories.Name

                }).ToList();

            return expense;
        }

        public Expense GetExpenseById(int id, int userId)
        {
            var expense = _expenseRepository.FindBy(x => x.Id == id && x.CreatedBy == userId).Select(x => new Expense
            {
                Id = x.Id,
                ViewDate = x.Date.Year.ToString() + "/" + x.Date.Month.ToString() + "/" + x.Date.Day.ToString(),
                Amount = x.Amount,
                CategoryId = x.CategoryId,
                CategoryName = x.Categories.Name,
                Description = x.Description,
                Item = x.Item,
                CreatedAt = x.CreatedAt,
                CreatedBy = x.CreatedBy,
                ModifiedAt = x.ModifiedAt

            }).FirstOrDefault();

            return expense;
        }

        public List<Expense> GetTodayExpense(int userId)
        {
            var expense = _expenseRepository.FindBy(x => x.CreatedBy == userId && x.Date == DateTime.Today).Select(x => new Expense
            {
                Id = x.Id,
                ViewDate = x.Date.Year.ToString() + "/" + x.Date.Month.ToString() + "/" + x.Date.Day.ToString(),
                Amount = x.Amount,
                CategoryId = x.CategoryId,
                CategoryName = x.Categories.Name,
                Description = x.Description,
                Item = x.Item,
                CreatedAt = x.CreatedAt,
                CreatedBy = x.CreatedBy,
                ModifiedAt = x.ModifiedAt

            }).ToList();

            return expense;
        }

        public List<Expense> GetExpenseReport(int userId, DateTime? startDate, DateTime? endDate, int? categoryId)
        {
            var expense = _expenseRepository.FindBy(x => x.CreatedBy == userId && (startDate == null || x.Date >= startDate) && (endDate == null || x.Date <= endDate) && (categoryId == null || x.CategoryId == categoryId))
                .OrderByDescending(x => x.Id).Select(x => new Expense
                {
                    Id = x.Id,
                    Amount = x.Amount,
                    Date = x.Date,
                    CategoryId = x.CategoryId,
                    CategoryName = x.Categories.Name,
                    Description = x.Description,
                    Item = x.Item,
                    CreatedAt = x.CreatedAt,
                    CreatedBy = x.CreatedBy,
                    ModifiedAt = x.ModifiedAt
                }).ToList();

            return expense;
        }

        public List<ExpenseChart> GetTotalAmountByDate(int userId)
        {
            var expenseChart = _expenseRepository.GetTotalAmountByDate(userId).Select(x => new ExpenseChart
            {
                date = x.date,
                totalAmount = x.totalAmount
            }).ToList();

            return expenseChart;
        }

        public List<ExpenseChartByCategory> GetMostExpensedCategory(int userId)
        {
            var categoryChart = _expenseRepository.GetMostExpensedCategory(userId).Select(x => new ExpenseChartByCategory
            {
                Count = x.count,
                CategoryName = x.Name
            }).ToList();

            return categoryChart;
        }
        #region Dashboard
        public int ExpenseCount(int userId)
        {
            var count = _expenseRepository.FindBy(x => x.CreatedBy == userId && x.Date == DateTime.Today).ToList().Count();
            return count;
        }

        public decimal GetTotalExpenseToday(int userId)
        {
            var amount = _expenseRepository.FindBy(x => x.CreatedBy == userId && x.Date == DateTime.Today).ToList().Sum(x => x.Amount);
            return amount;
        }

        public decimal GetTotalExpense(int userId)
        {
            var amount = _expenseRepository.FindBy(x => x.CreatedBy == userId).ToList().Sum(x => x.Amount);
            return amount;
        }

        #endregion
        #region CRUD
        public Message Create(Expense expense)
        {
            Message msg = new Message();
            try
            {
                var model = expense.ToDalEntity();
                _expenseRepository.Add(model);
                _expenseRepository.SaveChanges();
                msg.StatusCode = 200;
                msg.Status = "Expense Created Successfully!";
            }
            catch (Exception ex)
            {
                msg.StatusCode = 400;
                msg.Status = "An error occured while creating expense.";
            }
            return msg;
        }

        public Message Update(Expense expense)
        {
            Message msg = new Message();
            try
            {
                var model = _expenseRepository.FindBy(x => x.Id == expense.Id).FirstOrDefault();
                model.ModifiedAt = DateTime.Now;
                if (model == null)
                    throw new ArgumentException();
                expense.ToDalEntity(model);
                _expenseRepository.Edit(model);
                _expenseRepository.SaveChanges();
                msg.StatusCode = 200;
                msg.Status = "Category Updated Successfully!";
            }
            catch (Exception ex)
            {
                msg.StatusCode = 400;
                msg.Status = "An error occured while updating category.";
            }
            return msg;
        }

        public Message Delete(Expense expense)
        {
            Message msg = new Message();
            try
            {
                var model = _expenseRepository.FindBy(x => x.Id == expense.Id).FirstOrDefault();
                if (model == null)
                    throw new ArgumentException();
                expense.ToDalEntity(model);
                _expenseRepository.Delete(model);
                _expenseRepository.SaveChanges();
                msg.StatusCode = 200;
                msg.Status = "Expense Deleted Successfully!";
            }
            catch (Exception ex)
            {
                msg.StatusCode = 400;
                msg.Status = "There was an error deleting expense.";
            }

            return msg;
        }
        #endregion
    }
}
