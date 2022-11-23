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
        public IActionResult Index()
        {
            return View();
        }     
    }
}