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
            APIResponse? subcategoryResponse = await _subcategoryService.GetAllAsync<APIResponse>();

            if (subcategoryResponse != null && subcategoryResponse.isSuccess)
            {
                subcategoryList = JsonConvert.DeserializeObject<List<SubcategoryDTO>>(Convert.ToString(subcategoryResponse.Result));
            }

            return View(subcategoryList);
        }


        //GET
        public async Task<IActionResult> Create()
        {
            APIResponse? categoryResponse = await _categoryService.GetAllAsync<APIResponse>();
            if (categoryResponse != null && categoryResponse.isSuccess)
            {
                SubcategoryVM subcategoryVM = new SubcategoryVM()
                {
                    SubcategoryDTO = new SubcategoryDTO(),
                    CategoryList = JsonConvert.DeserializeObject<List<CategoryDTO>>(Convert.ToString(categoryResponse.Result))
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
                APIResponse? subcategoryResponse = await _subcategoryService.CreateAsync<APIResponse>(_mapper.Map<SubcategoryCreateDTO>(subcategoryVM.SubcategoryDTO));
                if (subcategoryResponse != null && subcategoryResponse.isSuccess)
                {
                    TempData["success"] = "Subcategory created successfully";
                    return RedirectToAction("Index");
                }
                if (subcategoryResponse.ErrorMessages.Count > 0)
                {
                    foreach (var error in subcategoryResponse.ErrorMessages)
                    {
                        ModelState.AddModelError("ErrorMessages", error);
                    }
                }
            }

            APIResponse? categoryResponse = await _categoryService.GetAllAsync<APIResponse>();
            if (categoryResponse != null && categoryResponse.isSuccess)
            {
                subcategoryVM.CategoryList = JsonConvert.DeserializeObject<List<CategoryDTO>>(Convert.ToString(categoryResponse.Result))
                .Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                });
            }
            return View(subcategoryVM);
        }


        //GET
        public async Task<IActionResult> Edit(int id)
        {
            APIResponse subcategoryResponse = await _subcategoryService.GetAsync<APIResponse>(id);
            APIResponse categoryResponse = await _categoryService.GetAllAsync<APIResponse>();

            if (subcategoryResponse != null && categoryResponse != null && subcategoryResponse.isSuccess && categoryResponse.isSuccess)
            {
                SubcategoryVM subcategoryVM = new SubcategoryVM()
                {
                    SubcategoryDTO = JsonConvert.DeserializeObject<SubcategoryDTO>(Convert.ToString(subcategoryResponse.Result)),
                    CategoryList = JsonConvert.DeserializeObject<List<CategoryDTO>>(Convert.ToString(categoryResponse.Result))
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
                APIResponse? subcategoryResponse = await _subcategoryService.UpdateAsync<APIResponse>(subcategoryVM.SubcategoryDTO);
                if (subcategoryResponse != null && subcategoryResponse.isSuccess)
                {
                    TempData["success"] = "Category updated successfully";
                    return RedirectToAction("Index");
                }
                if (subcategoryResponse.ErrorMessages.Count > 0)
                {
                    foreach (var error in subcategoryResponse.ErrorMessages)
                    {
                        ModelState.AddModelError("ErrorMessages", error);
                    }
                }
            }
            APIResponse? categoryResponse = await _categoryService.GetAllAsync<APIResponse>();
            if (categoryResponse != null && categoryResponse.isSuccess)
            {
                subcategoryVM.CategoryList = JsonConvert.DeserializeObject<List<CategoryDTO>>(Convert.ToString(categoryResponse.Result))
                .Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                });
            }
            return View(subcategoryVM);
        }


        //GET
        public async Task<IActionResult> Delete(int id)
        {
            APIResponse? subcategoryResponse = await _subcategoryService.GetAsync<APIResponse>(id);
            if (subcategoryResponse != null && subcategoryResponse.isSuccess)
            {
                SubcategoryDTO subcategoryDTO = JsonConvert.DeserializeObject<SubcategoryDTO>(Convert.ToString(subcategoryResponse.Result));
                return View(subcategoryDTO);
            }
            return NotFound();
        }


        //POST
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeletePOST(SubcategoryDTO subcategoryDTO)
        {
            APIResponse? subcategoryResponse = await _subcategoryService.DeleteAsync<APIResponse>(subcategoryDTO.Id);
            if (subcategoryResponse != null && subcategoryResponse.isSuccess)
            {
                TempData["success"] = "Category deleted successfully";
                return RedirectToAction("Index");
            }
            return NotFound();
        }      
    }
}
