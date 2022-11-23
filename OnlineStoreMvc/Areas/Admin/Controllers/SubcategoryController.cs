using AutoMapper;
using Azure;
using BulkyBookWeb.Areas.Admin.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Newtonsoft.Json;
using OnlineStore.DataAccess.Repository.IRepository;
using OnlineStore.Models;
using OnlineStore.Models.DTO;
using OnlineStore.Models.ViewModels;
using OnlineStoreMvc.Services.IServices;
using System.Collections.Generic;
using System.Globalization;

namespace OnlineStoreMvc.Areas.Admin.Controllers
{
    public class SubcategoryController : Controller
    {
        private readonly ISubcategoryService _subcategoryService;
        private readonly ICategoryService _categoryService;
        private readonly IMapper _mapper;

        public SubcategoryController(ISubcategoryService subcategoryService, ICategoryService categoryService, IMapper mapper)
        {
            _subcategoryService = subcategoryService;
            _categoryService = categoryService;
            _mapper = mapper;
        }


        public async Task<IActionResult> Index()
        {
            List<SubcategoryDTO> subcategoryList = new List<SubcategoryDTO>();
            var response = await _subcategoryService.GetAllAsync<APIResponse>();

            if (response != null && response.isSuccess)
            {
                subcategoryList = JsonConvert.DeserializeObject<List<SubcategoryDTO>>(Convert.ToString(response.Result));
            }
            return View(subcategoryList);
        }


        //GET
        public async Task<IActionResult> Create()
        {
            var response = await _categoryService.GetAllAsync<APIResponse>();
            if (response != null && response.isSuccess)
            {
                SubcategoryVM subcategoryVM = new SubcategoryVM()
                {
                    SubcategoryDTO = new SubcategoryDTO(),
                    CategoryList = JsonConvert.DeserializeObject<List<CategoryDTO>>(Convert.ToString(response.Result))
                    .Select(i => new SelectListItem
                    {
                        Text = i.Name,
                        Value = i.Id.ToString()
                    })
                };
                return View(subcategoryVM);
            }
            return NotFound();
        }

        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SubcategoryVM subcategoryVM)
        {
            if (ModelState.IsValid)
            {
                var response1 = await _subcategoryService.CreateAsync<APIResponse>(_mapper.Map<SubcategoryCreateDTO>(subcategoryVM.SubcategoryDTO));
                if (response1 != null && response1.isSuccess)
                {
                    TempData["success"] = "Subcategory created successfully";
                    return RedirectToAction("Index");
                }
                foreach (var error in response1.ErrorMessages)
                {
                    ModelState.AddModelError(error.Key, error.Value);
                }
            }

            var response2 = await _categoryService.GetAllAsync<APIResponse>();
            if (response2 != null && response2.isSuccess)
            {
                subcategoryVM.CategoryList = JsonConvert.DeserializeObject<List<CategoryDTO>>(Convert.ToString(response2.Result))
                .Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                });
                return View(subcategoryVM);
            }
            return NotFound();
        }


        //GET
        public async Task<IActionResult> Edit(int id)
        {
            APIResponse response1 = await _subcategoryService.GetAsync<APIResponse>(id);
            APIResponse response2 = await _categoryService.GetAllAsync<APIResponse>();

            if (response1.isSuccess && response2.isSuccess)
            {
                SubcategoryVM subcategoryVM = new SubcategoryVM()
                {
                    SubcategoryDTO = JsonConvert.DeserializeObject<SubcategoryDTO>(Convert.ToString(response1.Result)),
                    CategoryList = JsonConvert.DeserializeObject<List<CategoryDTO>>(Convert.ToString(response2.Result))
                    .Select(i => new SelectListItem
                    {
                        Text = i.Name,
                        Value = i.Id.ToString()
                    })
                };
                return View(subcategoryVM);
            }
            return RedirectToAction("Index");       
        }


        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(SubcategoryVM subcategoryVM)
        {
            if (ModelState.IsValid)
            {
                var response1 = await _subcategoryService.UpdateAsync<APIResponse>(subcategoryVM.SubcategoryDTO);
                if (response1 != null && response1.isSuccess)
                {
                    TempData["success"] = "Category updated successfully";
                    return RedirectToAction("Index");
                }

                foreach (var error in response1.ErrorMessages)
                {
                    ModelState.AddModelError(error.Key, error.Value);
                }
            }
            var response2 = await _categoryService.GetAllAsync<APIResponse>();
            if (response2 != null && response2.isSuccess)
            {
                subcategoryVM.CategoryList = JsonConvert.DeserializeObject<List<CategoryDTO>>(Convert.ToString(response2.Result))
                .Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                });
                return View(subcategoryVM);
            }
            return NotFound();
        }


        //GET
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _subcategoryService.GetAsync<APIResponse>(id);
            if (response != null && response.isSuccess)
            {
                SubcategoryDTO subcategoryDTO = JsonConvert.DeserializeObject<SubcategoryDTO>(Convert.ToString(response.Result));
                return View(subcategoryDTO);
            }
            return NotFound();
        }


        //POST
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeletePOST(SubcategoryDTO subcategoryDTO)
        {
            var response = await _subcategoryService.DeleteAsync<APIResponse>(subcategoryDTO.Id);
            if (response != null && response.isSuccess)
            {
                TempData["success"] = "Category deleted successfully";
                return RedirectToAction("Index");
            }
            return NotFound();
        }      
    }
}
