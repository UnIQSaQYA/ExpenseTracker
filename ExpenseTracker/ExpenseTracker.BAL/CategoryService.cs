using ExpenseTracker.Model;
using ExpensTracker.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace ExpenseTracker.BAL
{
    public class CategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        public CategoryService()
        {
            _categoryRepository = new CategoryRepository();
        }

        /// <summary>
        /// Returns List of Category of current login user, and if search string and status is provided
        /// returns list of category according to search string and status
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="searchString"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public List<Category> GetCategories(int userId, string searchString, int? status)
        {
            var category = _categoryRepository.FindBy(x => x.CreatedBy == userId && (searchString == null || x.Name.ToLower().Contains(searchString)) && (status == null || x.Status == status))
                .OrderByDescending(x => x.Id).Select(x => new Category
                {
                    Id = x.Id,
                    Name = x.Name,
                    Remarks = x.Remarks,
                    Status = x.Status,
                    CreatedAt = x.CreatedAt,
                    CreatedBy = x.CreatedBy
                }).ToList();
            return category;
        }

        public Category GetCategoryById(int id, int userId)
        {
            var category = _categoryRepository.FindBy(x => x.Id == id && x.CreatedBy == userId).Select(x => new Category
            {
                Id = x.Id,
                Name = x.Name,
                Status = x.Status,
                Remarks = x.Remarks,
                CreatedAt = x.CreatedAt,
                CreatedBy = x.CreatedBy,
                ModifiedAt = x.ModifiedAt
            }).FirstOrDefault();

            return category;
        }

        public int CategoryCount(int userId)
        {
            var count = _categoryRepository.FindBy(x => x.CreatedBy == userId).ToList().Count();
            return count;
        }

        #region CRUD
        public Message Create(Category category)
        {
            Message msg = new Message();
            try
            {
                var model = category.ToDalEntity();
                _categoryRepository.Add(model);
                _categoryRepository.SaveChanges();
                msg.StatusCode = 200;
                msg.Status = "Category Created Successfully!";
            }
            catch (Exception ex)
            {
                msg.StatusCode = 400;
                msg.Status = ex.Message;
            }

            return msg;
        }

        public Message Update(Category category)
        {
            Message msg = new Message();
            try
            {
                var model = _categoryRepository.FindBy(x=>x.Id == category.Id).FirstOrDefault();
                if (model == null)
                    throw new ArgumentException();
                category.ToDalEntity(model);
                _categoryRepository.Edit(model);
                _categoryRepository.SaveChanges();
                msg.StatusCode = 200;
                msg.Status = "Category Updated Successfully!";
            }
            catch(Exception ex)
            {
                msg.StatusCode = 400;
                msg.Status = "An error occured while updating category.";
            }
            return msg;
        }
        
        public Message Delete(Category category)
        {
            Message msg = new Message();
            try
            {
                var model = _categoryRepository.FindBy(x => x.Id == category.Id).FirstOrDefault();
                if (model == null)
                    throw new ArgumentException();
                category.ToDalEntity(model);
                _categoryRepository.Delete(model);
                _categoryRepository.SaveChanges();
                msg.StatusCode = 200;
                msg.Status = "Category Deleted Successfully!";
            }
            catch(Exception ex)
            {
                msg.StatusCode = 400;
                msg.Status = ex.Message;
            }

            return msg;
        }
        #endregion
    }
}
