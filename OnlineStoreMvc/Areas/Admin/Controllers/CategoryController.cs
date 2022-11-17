using OnlineStore.DataAccess;
using OnlineStore.Models;
using Microsoft.AspNetCore.Mvc;
using OnlineStore.DataAccess.Repository.IRepository;
using Microsoft.Extensions.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using Microsoft.CodeAnalysis.VisualBasic.Syntax;

namespace BulkyBookWeb.Areas.Admin.Controllers
{
    public class CategoryController : Controller
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly HttpClient _httpClient;
        public CategoryController(IHttpClientFactory factory, IRepositoryWrapper repositoryWrapper)
        {
            _httpClient = factory.CreateClient("test");
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("appplication/json"));
            _httpClient.BaseAddress = new Uri(_httpClient.BaseAddress + "/CategoryAPI");

            _repositoryWrapper = repositoryWrapper;
        }
        public IActionResult Index()
        {
            List<Category> categories = new List<Category>();

            HttpResponseMessage? response = _httpClient.GetAsync(_httpClient.BaseAddress).Result;
            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                categories = JsonConvert.DeserializeObject<List<Category>>(data);
            }
            return View(categories);
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
            HttpResponseMessage? response = _httpClient.PostAsJsonAsync(_httpClient.BaseAddress, category).Result;

            if (response.IsSuccessStatusCode)
            {
                TempData["success"] = "Category created successfully";
                return RedirectToAction("Index");
            }

            string data = response.Content.ReadAsStringAsync().Result;

            Dictionary<string, IList<string>>? errors = JsonConvert.DeserializeObject<Dictionary<string, IList<string>>>(data);

            foreach (var error in errors)
            {
                ModelState.AddModelError(error.Key, string.Join("", error.Value));
            }        
            return View(category);
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
            Category? categoryFromDb = null;

            HttpResponseMessage? response = _httpClient.GetAsync(_httpClient.BaseAddress + "/CategoryAPI/" + id.ToString()).Result;
            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                categoryFromDb = JsonConvert.DeserializeObject<Category>(data);
                return View(categoryFromDb);
            }
            return RedirectToAction("index");
        }

        //POST
        [/*HttpPost, */ActionName("Delete")]
        [ValidateAntiForgeryToken]  //protection, not necessary
        public IActionResult DeletePOST(Category _category)
        {
            Category? category = _repositoryWrapper.Category.GetFirstOrDefault(c => c.Id == _category.Id);
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
