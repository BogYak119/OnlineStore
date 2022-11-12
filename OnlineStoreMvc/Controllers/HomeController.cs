using Microsoft.AspNetCore.Mvc;
using OnlineStore.DataAccess.Repository.IRepository;
using OnlineStore.Models;
using OnlineStoreMvc.Models;
using System.Diagnostics;
using System.IO.Pipelines;
using System.Net.Http.Headers;

namespace OnlineStoreMvc.Controllers
{
    public class HomeController : Controller
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        public HomeController(IRepositoryWrapper repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
        }

        //private readonly ILogger<HomeController> _logger;

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult Test()
        {
            _repositoryWrapper.Clear();

            Category electronics = new Category { Name = "Electronics" };
            Category sports = new Category { Name = "Sports" };
            _repositoryWrapper.Category.AddRange(electronics, sports);
            _repositoryWrapper.Save();

            Subcategory phones = new Subcategory { Name = "Phones", CategoryId = _repositoryWrapper.Category.GetFirstOrDefault(c => c.Name == "Electronics").Id };
            Subcategory computers = new Subcategory { Name = "Computers", CategoryId = _repositoryWrapper.Category.GetFirstOrDefault(c => c.Name == "Electronics").Id };
            Subcategory fridges = new Subcategory { Name = "Fridges", CategoryId = _repositoryWrapper.Category.GetFirstOrDefault(c => c.Name == "Electronics").Id };
            Subcategory football = new Subcategory { Name = "Football", CategoryId = _repositoryWrapper.Category.GetFirstOrDefault(c => c.Name == "Sports").Id };
            Subcategory golf = new Subcategory { Name = "Golf", CategoryId = _repositoryWrapper.Category.GetFirstOrDefault(c => c.Name == "Sports").Id };
            _repositoryWrapper.Subcategory.AddRange(phones, computers, fridges, football, golf);
            _repositoryWrapper.Save();

            Manufacturer nokia = new Manufacturer { Name = "Nokia" };
            Manufacturer apple = new Manufacturer { Name = "Apple" };
            Manufacturer hp = new Manufacturer { Name = "Hewlett-Packard" };
            Manufacturer indesit = new Manufacturer { Name = "Indesit" };
            Manufacturer footballManufacurer = new Manufacturer { Name = "Football Manufacturer" };
            Manufacturer golfManufacturer = new Manufacturer { Name = "Golf Manufacturer" };
            _repositoryWrapper.Manufacturer.AddRange(nokia, apple, hp, indesit, footballManufacurer, golfManufacturer);
            _repositoryWrapper.Save();

            Product nokiaPhone = new Product
            {
                Name = "Nokia 1365",
                ManufacturerId = _repositoryWrapper.Manufacturer.GetFirstOrDefault(m => m.Name == "Nokia").Id,
                Price = 99.99M,
                Description = "A classic nokia phone",
                SubcategoryId = _repositoryWrapper.Subcategory.GetFirstOrDefault(m => m.Name == "Phones").Id
            };
            _repositoryWrapper.Product.Add(nokiaPhone);
            _repositoryWrapper.Save();

            return View(_repositoryWrapper);
        }
        public IActionResult AddManufacturers()
        {
            Manufacturer nokia = new Manufacturer { Name = "Nokia" };
            Manufacturer apple = new Manufacturer { Name = "Apple" };
            Manufacturer hp = new Manufacturer { Name = "Hewlett-Packard" };
            Manufacturer indesit = new Manufacturer { Name = "Indesit" };
            Manufacturer footbalMan = new Manufacturer { Name = "Football Manufacturer" };
            Manufacturer golfManufacturer = new Manufacturer { Name = "Golf Manufacturer" };

            return View("Index");
        }
    }
}