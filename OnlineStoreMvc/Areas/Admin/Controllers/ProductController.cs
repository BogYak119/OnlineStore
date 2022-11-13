using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using OnlineStore.DataAccess.Repository.IRepository;
using OnlineStore.Models.ViewModels;
using OnlineStore.Models;
using System.Globalization;

namespace OnlineStoreMvc.Areas.Admin.Controllers
{
    public class ProductController : Controller
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        public ProductController(IRepositoryWrapper repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
        }
        public IActionResult Index()
        {
            IEnumerable<Product> productList = _repositoryWrapper.Product.GetAll();
            return View(productList);
        }

        //GET
        public IActionResult Create()
        {
            ProductVM productVM = new ProductVM()
            {
                Product = new Product(),
                CategoryList = _repositoryWrapper.Category.GetAll().Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                }),
                SubcategoryList = _repositoryWrapper.Subcategory.GetAll().Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                }),
                ManufacturerList = _repositoryWrapper.Manufacturer.GetAll().Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                })
            };
            return View(productVM);
        }

        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]  //protection, not necessary
        public IActionResult Create(ProductVM productVM)
        {
            if (_repositoryWrapper.Product.GetFirstOrDefault(p => p.Name == productVM.Product.Name) != null)
            {
                ModelState.AddModelError("Product.Name", "Database already contains product with the same name.");
            }

            if (ModelState.IsValid)
            {
                productVM.Product.Subcategory.Name = _repositoryWrapper.Subcategory.GetFirstOrDefault(sc => sc.Id == productVM.Product.SubcategoryId).Name;
                _repositoryWrapper.Product.Add(productVM.Product);
                _repositoryWrapper.Save();
                TempData["success"] = "Product created successfully";
                return RedirectToAction("Index");
            }

            productVM.CategoryList = _repositoryWrapper.Category.GetAll().Select(i => new SelectListItem
            {
                Text = i.Name,
                Value = i.Id.ToString()
            });
            productVM.SubcategoryList = _repositoryWrapper.Subcategory.GetAll().Select(i => new SelectListItem
            {
                Text = i.Name,
                Value = i.Id.ToString()
            });
            productVM.ManufacturerList = _repositoryWrapper.Manufacturer.GetAll().Select(i => new SelectListItem
            {
                Text = i.Name,
                Value = i.Id.ToString()
            });
            return View(productVM);
        }

        //GET
        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            ProductVM productVM = new ProductVM()
            {
                Product = _repositoryWrapper.Product.GetFirstOrDefault(p => p.Id == id),
                CategoryList = _repositoryWrapper.Category.GetAll().Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                }),
                SubcategoryList = _repositoryWrapper.Subcategory.GetAll().Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                }),
                ManufacturerList = _repositoryWrapper.Manufacturer.GetAll().Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                })
            };
            if (productVM.Product == null)
            {
                return NotFound();
            }

            return View(productVM);
        }

        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]  //protection, not necessary
        public IActionResult Edit(ProductVM productVM)
        {
            bool newNameIsExists = _repositoryWrapper.Product.GetFirstOrDefault(sc => sc.Name == productVM.Product.Name) != null;

            if(_repositoryWrapper.Product.GetFirstOrDefault(sc => sc.Name == productVM.Product.Name) != null)
            {
                ModelState.AddModelError("Product.Name", "Database already contains product with the same name.");
            }

            if (ModelState.IsValid)
            {
                _repositoryWrapper.Product.Update(productVM.Product);
                _repositoryWrapper.Save();
                TempData["success"] = "Product updated successfully";
                return RedirectToAction("Index");
            }

            productVM.CategoryList = _repositoryWrapper.Category.GetAll().Select(i => new SelectListItem
            {
                Text = i.Name,
                Value = i.Id.ToString()
            });

            return View(productVM);
        }

        //GET
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Product? subcategoryFromDb = _repositoryWrapper.Product.GetFirstOrDefault(c => c.Id == id);

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
            Product? subcategory = _repositoryWrapper.Product.GetFirstOrDefault(c => c.Id == id);
            if (subcategory == null)
            {
                return NotFound();
            }

            _repositoryWrapper.Product.Remove(subcategory);
            _repositoryWrapper.Save();
            TempData["success"] = "Product deleted successfully";
            return RedirectToAction("Index");
        }
    }
}
