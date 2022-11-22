using Microsoft.AspNetCore.Mvc;
using OnlineStore.DataAccess.Repository.IRepository;
using OnlineStore.Models;

namespace OnlineStoreMvc.Areas.Admin.Controllers
{
    public class ManufacturerController : Controller
    {
        //private readonly IRepositoryWrapper _repositoryWrapper;
        //public ManufacturerController(IRepositoryWrapper repositoryWrapper)
        //{
        //    _repositoryWrapper = repositoryWrapper;
        //}
        //public IActionResult Index()
        //{
        //    IEnumerable<Manufacturer> manufacturerList = _repositoryWrapper.Manufacturer.GetAll();
        //    return View(manufacturerList);
        //}

        ////GET
        //public IActionResult Create()
        //{
        //    return View();
        //}

        ////POST
        //[HttpPost]
        //[ValidateAntiForgeryToken]  //protection, not necessary
        //public IActionResult Create(Manufacturer manufacturer)
        //{
        //    if (_repositoryWrapper.Manufacturer.GetFirstOrDefault(m => m.Name == manufacturer.Name) != null)
        //    {
        //        ModelState.AddModelError("name", "Database already contains manufacturer with the same name.");
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        _repositoryWrapper.Manufacturer.Add(manufacturer);
        //        _repositoryWrapper.Save();
        //        TempData["success"] = "Manufacturer created successfully";
        //        return RedirectToAction("Index");
        //    }
        //    else return View(manufacturer);
        //}

        ////GET
        //public IActionResult Edit(int? id)
        //{
        //    if (id == null || id == 0)
        //    {
        //        return NotFound();
        //    }

        //    Manufacturer? manufacturerFromDb = _repositoryWrapper.Manufacturer.GetFirstOrDefault(m => m.Id == id);

        //    if (manufacturerFromDb == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(manufacturerFromDb);
        //}

        ////POST
        //[HttpPost]
        //[ValidateAntiForgeryToken]  //protection, not necessary
        //public IActionResult Edit(Manufacturer manufacturer)
        //{
        //    if (_repositoryWrapper.Manufacturer.GetFirstOrDefault(c => c.Name == manufacturer.Name) != null)
        //    {
        //        ModelState.AddModelError("name", "Database already contains manufacturer with the same name.");
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        _repositoryWrapper.Manufacturer.Update(manufacturer);
        //        _repositoryWrapper.Save();
        //        TempData["success"] = "Manufacturer updated successfully";
        //        return RedirectToAction("Index");
        //    }
        //    else return View(manufacturer);
        //}

        ////GET
        //public IActionResult Delete(int? id)
        //{
        //    if (id == null || id == 0)
        //    {
        //        return NotFound();
        //    }
        //    Manufacturer? manufacturerFromDb = _repositoryWrapper.Manufacturer.GetFirstOrDefault(c => c.Id == id);

        //    if (manufacturerFromDb == null)
        //    {
        //        return NotFound();
        //    }
        //    return View(manufacturerFromDb);
        //}

        ////POST
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]  //protection, not necessary
        //public IActionResult DeletePOST(int? id)
        //{
        //    Manufacturer? manufacturer = _repositoryWrapper.Manufacturer.GetFirstOrDefault(c => c.Id == id);
        //    if (manufacturer == null)
        //    {
        //        return NotFound();
        //    }

        //    _repositoryWrapper.Manufacturer.Remove(manufacturer);
        //    _repositoryWrapper.Save();
        //    TempData["success"] = "Manufacturer deleted successfully";
        //    return RedirectToAction("Index");
        //}
    }
}
