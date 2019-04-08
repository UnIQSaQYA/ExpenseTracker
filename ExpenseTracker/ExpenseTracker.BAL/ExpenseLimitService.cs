using ExpenseTracker.Model;
using ExpensTracker.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.BAL
{
    public class ExpenseLimitService
    {
        private readonly IExpenseLimitRepository _expenseLimitRepository;
        public ExpenseLimitService()
        {
            _expenseLimitRepository = new ExpenseLimitRepository();
        }

        public ExpenseLimit GetExpenseByUser(int userId)
        {
            var expenseLimit = _expenseLimitRepository.FindBy(x => x.CreatedBy == userId).Select(x => new ExpenseLimit
            {
                Id = x.Id,
                LimitAmount = x.LimitAmount,
                Status = x.Status,
                CreatedBy = x.CreatedBy
            }).FirstOrDefault();
            return expenseLimit;
        }

        #region CRUD
        public Message Create(ExpenseLimit expenseLimit)
        {
            Message msg = new Message();
            try
            {
                var model = expenseLimit.ToDalEntity();
                _expenseLimitRepository.Add(model);
                _expenseLimitRepository.SaveChanges();
                msg.StatusCode = 200;
                msg.Status = "Expense limit has been added successfully. You will be notified when you expend more than your Limit";
            }
            catch (Exception ex)
            {
                msg.StatusCode = 400;
                msg.Status = "An error occured while creating expense limit.";
            }

            return msg;
        }

        public Message Update(ExpenseLimit expenseLimit)
        {
            var msg = new Message();
            try
            {
                var model = _expenseLimitRepository.FindBy(x => x.CreatedBy == expenseLimit.CreatedBy).FirstOrDefault();
                if (model == null)
                    throw new ArgumentException();
                expenseLimit.ToDalEntity(model);
                _expenseLimitRepository.Edit(model);
                _expenseLimitRepository.SaveChanges();
                msg.StatusCode = 200;
                msg.Status = "Expense Limit Updated Successfully";
            }
            catch (Exception ex)
            {
                msg.StatusCode = 400;
                msg.Status = "An error occured while creating expense limit.";
            }
            return msg;
        }
        #endregion
    }
}
