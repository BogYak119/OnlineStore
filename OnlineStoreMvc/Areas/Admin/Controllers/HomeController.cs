using Microsoft.AspNetCore.Mvc;
using OnlineStore.DataAccess.Repository.IRepository;
using OnlineStore.Models;
using OnlineStoreMvc.Models;
using System.Diagnostics;
using System.IO.Pipelines;
using System.Net.Http.Headers;

namespace OnlineStoreMvc.Areas.Admin.Controllers
{
    public class HomeController : Controller
    {
        //private readonly IRepositoryWrapper _repositoryWrapper;
        //public HomeController(IRepositoryWrapper repositoryWrapper)
        //{
        //    _repositoryWrapper = repositoryWrapper;
        //}

        ////private readonly ILogger<HomeController> _logger;

        public IActionResult Index()
        {
            return View();
        }

        //public IActionResult Privacy()
        //{
        //    return View();
        //}

        //[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        //public IActionResult Error()
        //{
        //    return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        //}

        //public IActionResult AddManufacturers()
        //{
        //    Manufacturer nokia = new Manufacturer { Name = "Nokia" };
        //    Manufacturer apple = new Manufacturer { Name = "Apple" };
        //    Manufacturer hp = new Manufacturer { Name = "Hewlett-Packard" };
        //    Manufacturer indesit = new Manufacturer { Name = "Indesit" };
        //    Manufacturer footbalMan = new Manufacturer { Name = "Football Manufacturer" };
        //    Manufacturer golfManufacturer = new Manufacturer { Name = "Golf Manufacturer" };

        //    return View("Index");
        //}

        ////GET
        //public IActionResult Test2()
        //{
        //    TestModel model = new TestModel
        //    {
        //        list = new List<string> { "data", "from", "list"},
        //        array = new[] { "data", "from", "array" },
        //        Input = "string input"
        //    };
        //    return View(model);
        //}

        //[HttpPost]
        //public IActionResult Test2(TestModel model)
        //{
        //    Console.ReadLine();
        //    return View("Index");
        //}
    }
}