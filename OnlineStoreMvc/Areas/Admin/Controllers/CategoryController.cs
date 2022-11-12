using OnlineStore.DataAccess;
using OnlineStore.Models;
using Microsoft.AspNetCore.Mvc;
using OnlineStore.DataAccess.Repository.IRepository;

namespace BulkyBookWeb.Areas.Admin.Controllers
{
    public class CategoryController : Controller
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        public CategoryController(IRepositoryWrapper repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
        }
        public IActionResult Index()
        {
            IEnumerable<Category> categoryList = _repositoryWrapper.Category.GetAll();
            return View(categoryList);
        }

        //GET
        public IActionResult Create()
        {
            return View();
        }

        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]  //protection, not necessary
        public IActionResult Create(Category category)
        {
            if (_repositoryWrapper.Category.GetFirstOrDefault(c => c.Name == category.Name) != null)
            {
                ModelState.AddModelError("name", "Database already contains category with the same name.");
            }

            if (ModelState.IsValid)
            {
                _repositoryWrapper.Category.Add(category);
                _repositoryWrapper.Save();
                TempData["success"] = "Category created successfully";
                return RedirectToAction("Index");
            }
            else return View(category);
        }

        //GET
        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            Category? categoryFromDb = _repositoryWrapper.Category.GetFirstOrDefault(u => u.Id == id);

            if (categoryFromDb == null)
            {
                return NotFound();
            }

            return View(categoryFromDb);
        }

        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]  //protection, not necessary
        public IActionResult Edit(Category category)
        {
            if (_repositoryWrapper.Category.GetFirstOrDefault(c => c.Name == category.Name) != null)
            {
                ModelState.AddModelError("name", "Database already contains category with the same name.");
            }

            if (ModelState.IsValid)
            {
                _repositoryWrapper.Category.Update(category);
                _repositoryWrapper.Save();
                TempData["success"] = "Category updated successfully";
                return RedirectToAction("Index");
            }
            else return View(category);
        }

        //GET
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Category? categoryFromDb = _repositoryWrapper.Category.GetFirstOrDefault(c => c.Id == id);

            if (categoryFromDb == null)
            {
                return NotFound();
            }
            return View(categoryFromDb);
        }

        //POST
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]  //protection, not necessary
        public IActionResult DeletePOST(int? id)
        {
            Category? category = _repositoryWrapper.Category.GetFirstOrDefault(c => c.Id == id);
            if (category == null)
            {
                return NotFound();
            }

            _repositoryWrapper.Category.Remove(category);
            _repositoryWrapper.Save();
            TempData["success"] = "Category deleted successfully";
            return RedirectToAction("Index");
        }
    }
}
