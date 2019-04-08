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
    public class CategoryController : BaseController
    {
        CategoryService _categoryService = new CategoryService();
        // GET: Category
        /// <summary>
        /// Returns category index view, returns partial view while ajax call
        /// </summary>
        /// <param name="searchString"></param>
        /// <param name="status"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public ActionResult Index(string searchString, int? status, int page = 1)
        {
            //Gets user session data
            ExpenseTrackerIdentity ident = User.Identity as ExpenseTrackerIdentity;

            var category = _categoryService.GetCategories(ident.UserId, searchString, status);
            return Request.IsAjaxRequest()
             ? (ActionResult)PartialView("_list", category.ToPagedList(page, pageSize))
             : View(category.ToPagedList(page, pageSize));
        }

        public ActionResult Create()
        {
            return View();
        }

        /// <summary>
        ///  Create Category of current session user
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Category model)
        {
            if (ModelState.IsValid)
            {
                //Get user id from session using identity
                ExpenseTrackerIdentity ident = User.Identity as ExpenseTrackerIdentity;
                model.CreatedBy = ident.UserId;
                model.CreatedAt = DateTime.Now;
                var data = _categoryService.Create(model);
                //displays both error and success message depending on statuscode
                ShowStatus(data.StatusCode, data.Status);

                //if insert is successful return to index page else display an error and keep user at create page and diplays error
                if(data.StatusCode == 200)
                    return RedirectToAction("Index", "Category");
            }
            return View(model);
        }
        /// <summary>
        /// displays edit page else with data, if id not found redirects to index page with error message
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Edit(int id)
        {
            ExpenseTrackerIdentity ident = User.Identity as ExpenseTrackerIdentity;
            var category = _categoryService.GetCategoryById(id, ident.UserId);
            if(category == null)
            {
                ShowStatus(400, "Category with id " + id.ToString() + " does not exist.");
                return RedirectToAction("Index", "Category");
            }
            return View(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Category category)
        {
            if (ModelState.IsValid)
            {
                var data = _categoryService.Update(category);
                ShowStatus(data.StatusCode, data.Status);
                if (data.StatusCode == 200)
                    return RedirectToAction("Index", "Category");
            }
            return View(category);
        }

        /// <summary>
        /// opens confirmation modal for delete
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult DeleteConfirmation(int id)
        {
            ExpenseTrackerIdentity ident = User.Identity as ExpenseTrackerIdentity;
            var category = _categoryService.GetCategoryById(id, ident.UserId);
            if (category == null)
            {
                ShowStatus(400, "Category with id " + id.ToString() + " does not exist.");
                return RedirectToAction("Index", "Category");
            }

            return PartialView("_deleteConfirmation", category);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(Category category)
        {

            var data = _categoryService.Delete(category);
            ShowStatus(data.StatusCode, data.Status);
            return RedirectToAction("Index", "Category");
        }
    }
}