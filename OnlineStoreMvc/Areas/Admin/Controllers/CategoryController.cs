using OnlineStore.DataAccess;
using OnlineStore.Models;
using Microsoft.AspNetCore.Mvc;
using OnlineStore.DataAccess.Repository.IRepository;
using Microsoft.Extensions.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using Microsoft.CodeAnalysis.VisualBasic.Syntax;
using OnlineStoreMvc.Services.IServices;
using AutoMapper;
using OnlineStore.Models.DTO;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace BulkyBookWeb.Areas.Admin.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategoryService _categoryService;
        private readonly IMapper _mapper;

        public CategoryController(ICategoryService categoryService, IMapper mapper)
        {
            _categoryService = categoryService;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            List<CategoryDTO> categoryList = new List<CategoryDTO>();
            var response = await _categoryService.GetAllAsync<APIResponse>();

            if (response != null && response.isSuccess)
            {
                categoryList = JsonConvert.DeserializeObject<List<CategoryDTO>>(Convert.ToString(response.Result));
            }
            return View(categoryList);
        }

        //GET
        public IActionResult Create()
        {
            return View();
        }

        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CategoryCreateDTO categoryCreateDTO)
        {
            if (ModelState.IsValid)
            {
                var response = await _categoryService.CreateAsync<APIResponse>(categoryCreateDTO);
                if (response != null && response.isSuccess)
                {
                    TempData["success"] = "Category created successfully";
                    return RedirectToAction("Index");
                }

                foreach (var error in response.ErrorMessages)
                {
                    ModelState.AddModelError(error.Key, error.Value);
                }
            }
            return View(categoryCreateDTO);
        }

        //GET
        public async Task<IActionResult> Edit(int id)
        {
            var response = await _categoryService.GetAsync<APIResponse>(id);
            if (response != null && response.isSuccess)
            {
                CategoryDTO categoryDTO = JsonConvert.DeserializeObject<CategoryDTO>(Convert.ToString(response.Result));
                return View(categoryDTO);
            }
            return NotFound();
        }

        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(CategoryDTO categoryDTO)
        {
            if (ModelState.IsValid)
            {
                var response = await _categoryService.UpdateAsync<APIResponse>(categoryDTO);
                if (response != null && response.isSuccess)
                {
                    TempData["success"] = "Category updated successfully";
                    return RedirectToAction("Index");
                }

                foreach (var error in response.ErrorMessages)
                {
                    ModelState.AddModelError(error.Key, error.Value);
                }
            }
            return View(categoryDTO);
        }

        //GET
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _categoryService.GetAsync<APIResponse>(id);
            if (response != null && response.isSuccess)
            {
                CategoryDTO categoryDTO = JsonConvert.DeserializeObject<CategoryDTO>(Convert.ToString(response.Result));
                return View(categoryDTO);
            }
            return NotFound();
        }

        //POST
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]  //protection, not necessary
        public async Task<IActionResult> DeletePOST(CategoryDTO categoryDTO)
        {
            var response = await _categoryService.DeleteAsync<APIResponse>(categoryDTO.Id);
            if (response != null && response.isSuccess)
            {
                TempData["success"] = "Category deleted successfully";
                return RedirectToAction("Index");
            }          
            return NotFound();
        }
    }
}
