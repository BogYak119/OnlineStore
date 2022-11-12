using BulkyBookWeb.Areas.Admin.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using OnlineStore.DataAccess.Repository.IRepository;
using OnlineStore.Models;
using OnlineStore.Models.ViewModels;
using System.Collections.Generic;

namespace OnlineStoreMvc.Areas.Admin.Controllers
{
    public class SubcategoryController : Controller
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        public SubcategoryController(IRepositoryWrapper repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
        }
        public IActionResult Index()
        {
            IEnumerable<Subcategory> subcategoryList = _repositoryWrapper.Subcategory.GetAll();
            return View(subcategoryList);
        }

        //GET
        public IActionResult Create()
        {
            SubcategoryVM subcategoryVM = new SubcategoryVM()
            {
                Subcategory = new Subcategory(),
                CategoryList = _repositoryWrapper.Category.GetAll().Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                })
            };
            return View(subcategoryVM);
        }

        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]  //protection, not necessary
        public IActionResult Create(SubcategoryVM subcategoryVM)
        {
            if (_repositoryWrapper.Subcategory.GetFirstOrDefault(sc => sc.Name == subcategoryVM.Subcategory.Name) != null)
            {
                ModelState.AddModelError("Subcategory.Name", "Database already contains subcategory with the same name.");
            }

            if (ModelState.IsValid)
            {
                _repositoryWrapper.Subcategory.Add(subcategoryVM.Subcategory);
                _repositoryWrapper.Save();
                TempData["success"] = "Subcategory created successfully";
                return RedirectToAction("Index");
            }

            subcategoryVM.CategoryList = _repositoryWrapper.Category.GetAll().Select(i => new SelectListItem
            {
                Text = i.Name,
                Value = i.Id.ToString()
            });

            return View(subcategoryVM);
        }

        //GET
        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            SubcategoryVM subcategoryVM = new SubcategoryVM()
            {
                Subcategory = _repositoryWrapper.Subcategory.GetFirstOrDefault(u => u.Id == id),
                CategoryList = _repositoryWrapper.Category.GetAll().Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                })
            };
            if (subcategoryVM.Subcategory == null)
            {
                return NotFound();
            }

            TempData["prevName"] = subcategoryVM.Subcategory.Name;
            TempData["prevId"] = subcategoryVM.Subcategory.CategoryId;

            return View(subcategoryVM);
        }

        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]  //protection, not necessary
        public IActionResult Edit(SubcategoryVM subcategoryVM)
        {
            //dont forget to make normal validation, move some validation to client side !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
            bool nameIsChanged = subcategoryVM.Subcategory.Name != TempData["prevName"] as string;
            bool categoryIsChanged = subcategoryVM.Subcategory.CategoryId != TempData["prevID"] as int?;
            bool newNameIsExists = _repositoryWrapper.Subcategory.GetFirstOrDefault(sc => sc.Name == subcategoryVM.Subcategory.Name) != null;
            bool stopCheck = false;            

            if(categoryIsChanged && !nameIsChanged)
            {
                stopCheck = true;
            }
            if(!stopCheck && newNameIsExists && categoryIsChanged)
            {
                ModelState.AddModelError("Subcategory.Name", "Database already contains subcategory with the same name.");
                stopCheck = true;
            }
            if(categoryIsChanged && !nameIsChanged && !newNameIsExists)
            {
                stopCheck = true;
            }
            if (!stopCheck && !categoryIsChanged && !nameIsChanged)
            {
                ModelState.AddModelError("NoDataChange", "Data didn't change");
                stopCheck = true;
            }
            if(!stopCheck && !categoryIsChanged && nameIsChanged && newNameIsExists)
            {
                ModelState.AddModelError("Subcategory.Name", "Database already contains subcategory with the same name.");
                stopCheck = true;
            }
            if(!stopCheck && categoryIsChanged && nameIsChanged && newNameIsExists)
            {
                ModelState.AddModelError("Subcategory.Name", "Database already contains subcategory with the same name.");
                stopCheck = true;
            }


            if (ModelState.IsValid)
            {
                _repositoryWrapper.Subcategory.Update(subcategoryVM.Subcategory);
                _repositoryWrapper.Save();
                TempData["success"] = "Subcategory updated successfully";
                return RedirectToAction("Index");
            }

            subcategoryVM.CategoryList = _repositoryWrapper.Category.GetAll().Select(i => new SelectListItem
            {
                Text = i.Name,
                Value = i.Id.ToString()
            });

            TempData["prevName"] = subcategoryVM.Subcategory.Name;
            TempData["prevId"] = subcategoryVM.Subcategory.CategoryId;
            return View(subcategoryVM);
        }

        //GET
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Subcategory? subcategoryFromDb = _repositoryWrapper.Subcategory.GetFirstOrDefault(c => c.Id == id);

            if (subcategoryFromDb == null)
            {
                return NotFound();
            }
            return View(subcategoryFromDb);
        }

        //POST
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]  //protection, not necessary
        public IActionResult DeletePOST(int? id)
        {
            Subcategory? subcategory = _repositoryWrapper.Subcategory.GetFirstOrDefault(c => c.Id == id);
            if (subcategory == null)
            {
                return NotFound();
            }

            _repositoryWrapper.Subcategory.Remove(subcategory);
            _repositoryWrapper.Save();
            TempData["success"] = "Subcategory deleted successfully";
            return RedirectToAction("Index");
        }
    }
}
