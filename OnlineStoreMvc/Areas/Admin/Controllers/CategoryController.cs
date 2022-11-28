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
using Microsoft.AspNetCore.Authorization;

namespace BulkyBookWeb.Areas.Admin.Controllers
{   
    [Authorize(Roles = "admin")]
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
            APIResponse? categoryResponse = await _categoryService.GetAllAsync<APIResponse>();

            if (categoryResponse != null && categoryResponse.isSuccess)
            {
                categoryList = JsonConvert.DeserializeObject<List<CategoryDTO>>(Convert.ToString(categoryResponse.Result));
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
                APIResponse? categoryResponse = await _categoryService.CreateAsync<APIResponse>(categoryCreateDTO);
                if (categoryResponse != null && categoryResponse.isSuccess)
                {
                    TempData["success"] = "Category created successfully";
                    return RedirectToAction("Index");
                }

                if(categoryResponse.ErrorMessages.Count > 0)
                {
                    foreach (var error in categoryResponse.ErrorMessages)
                    {
                        ModelState.AddModelError("ErrorMessages", error);
                    }
                }
            }
            return View(categoryCreateDTO);
        }


        //GET
        public async Task<IActionResult> Edit(int id)
        {
            APIResponse? categoryResponse = await _categoryService.GetAsync<APIResponse>(id);
            if (categoryResponse != null && categoryResponse.isSuccess)
            {
                CategoryDTO categoryDTO = JsonConvert.DeserializeObject<CategoryDTO>(Convert.ToString(categoryResponse.Result));
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
                var categoryResponse = await _categoryService.UpdateAsync<APIResponse>(categoryDTO);
                if (categoryResponse != null && categoryResponse.isSuccess)
                {
                    TempData["success"] = "Category updated successfully";
                    return RedirectToAction("Index");
                }

                if (categoryResponse.ErrorMessages.Count > 0)
                {
                    foreach (var error in categoryResponse.ErrorMessages)
                    {
                        ModelState.AddModelError("ErrorMessages", error);
                    }
                }
                //foreach (var error in categoryResponse.ErrorMessages)
                //{
                //    ModelState.AddModelError(error.Key, error.Value);
                //}
            }
            return View(categoryDTO);
        }


        //GET
        public async Task<IActionResult> Delete(int id)
        {
            APIResponse? categoryResponse = await _categoryService.GetAsync<APIResponse>(id);
            if (categoryResponse != null && categoryResponse.isSuccess)
            {
                CategoryDTO categoryDTO = JsonConvert.DeserializeObject<CategoryDTO>(Convert.ToString(categoryResponse.Result));
                return View(categoryDTO);
            }
            return NotFound();
        }


        //POST
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken] 
        public async Task<IActionResult> DeletePOST(CategoryDTO categoryDTO)
        {
            var categoryResponse = await _categoryService.DeleteAsync<APIResponse>(categoryDTO.Id);
            if (categoryResponse != null && categoryResponse.isSuccess)
            {
                TempData["success"] = "Category deleted successfully";
                return RedirectToAction("Index");
            }          
            return NotFound();
        }
    }
}
