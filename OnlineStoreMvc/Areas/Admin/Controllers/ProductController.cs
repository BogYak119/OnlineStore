﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using OnlineStore.DataAccess.Repository.IRepository;
using OnlineStore.Models.ViewModels;
using OnlineStore.Models;
using System.Globalization;
using AutoMapper;
using Newtonsoft.Json;
using OnlineStore.Models.DTO;
using OnlineStoreMvc.Services.IServices;
using OnlineStoreMvc.Services;
using Microsoft.AspNetCore.Authorization;
using System.Data;
using OnlineStore.Utility;

namespace OnlineStoreMvc.Areas.Admin.Controllers
{
    [Authorize(Roles = "admin")]
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        private readonly IManufacturerService _manufacturerService;
        private readonly ISubcategoryService _subcategoryService;
        private readonly ICategoryService _categoryService;
        private readonly IMapper _mapper;

        public ProductController(IProductService productService, IManufacturerService manufacturerService, ISubcategoryService subcategoryService, ICategoryService categoryService, IMapper mapper)
        {
            _productService = productService;
            _manufacturerService = manufacturerService;
            _subcategoryService = subcategoryService;
            _categoryService = categoryService;
            _mapper = mapper;
        }


        public async Task<IActionResult> Index()
        {
            List<ProductDTO> productList = new List<ProductDTO>();
            APIResponse? response = await _productService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SessionToken));

            if (response != null && response.isSuccess)
            {
                productList = JsonConvert.DeserializeObject<List<ProductDTO>>(Convert.ToString(response.Result));
            }
            return View(productList);
        }


        //GET
        public async Task<IActionResult> Create()
        {
            APIResponse? manufacturerResponse = await _manufacturerService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SessionToken));
            APIResponse? subcategoryResponse = await _subcategoryService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SessionToken));
            if (manufacturerResponse != null && manufacturerResponse.isSuccess
                && subcategoryResponse != null && subcategoryResponse.isSuccess)
            {
                ProductVM productVM = new ProductVM()
                {
                    ProductDTO = new ProductDTO(),
                    ManufacturerList = JsonConvert.DeserializeObject<List<ManufacturerDTO>>(Convert.ToString(manufacturerResponse.Result))
                    .Select(i => new SelectListItem
                    {
                        Text = i.Name,
                        Value = i.Id.ToString()
                    }),
                    SubcategoryList = JsonConvert.DeserializeObject<List<SubcategoryDTO>>(Convert.ToString(subcategoryResponse.Result))
                    .Select(i => new SelectListItem
                    {
                        Text = i.Name,
                        Value = i.Id.ToString()
                    })
                };
                return View(productVM);
            }
            return NotFound();
        }


        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductVM productVM)
        {
            if (ModelState.IsValid)
            {
                APIResponse? productResponse = await _productService.CreateAsync<APIResponse>(_mapper.Map<ProductCreateDTO>(productVM.ProductDTO), HttpContext.Session.GetString(SD.SessionToken));
                if (productResponse != null && productResponse.isSuccess)
                {
                    TempData["success"] = "Product created successfully";
                    return RedirectToAction("Index");
                }
                if (productResponse.ErrorMessages.Count > 0)
                {
                    foreach (var error in productResponse.ErrorMessages)
                    {
                        ModelState.AddModelError("ErrorMessages", error);
                    }
                }
            }

            APIResponse? manufacturerResponse = await _manufacturerService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SessionToken));
            APIResponse? subcategoryResponse = await _subcategoryService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SessionToken));
            if (manufacturerResponse != null && manufacturerResponse.isSuccess
                 && subcategoryResponse != null && subcategoryResponse.isSuccess)
            {
                productVM.ManufacturerList = JsonConvert.DeserializeObject<List<ManufacturerDTO>>(Convert.ToString(manufacturerResponse.Result))
                    .Select(i => new SelectListItem
                    {
                        Text = i.Name,
                        Value = i.Id.ToString()
                    });
                productVM.SubcategoryList = JsonConvert.DeserializeObject<List<SubcategoryDTO>>(Convert.ToString(subcategoryResponse.Result))
                    .Select(i => new SelectListItem
                    {
                        Text = i.Name,
                        Value = i.Id.ToString()
                    });
                return View(productVM);
            }
            return RedirectToAction("Index");
        }


        //GET
        public async Task<IActionResult> Edit(int id)
        {
            APIResponse? productResponse = await _productService.GetAsync<APIResponse>(id, HttpContext.Session.GetString(SD.SessionToken));
            APIResponse? manufacturerResponse = await _manufacturerService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SessionToken));
            APIResponse? subcategoryResponse = await _subcategoryService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SessionToken));
            if (productResponse != null && productResponse.isSuccess
                && manufacturerResponse != null && manufacturerResponse.isSuccess
                && subcategoryResponse != null && subcategoryResponse.isSuccess)
            {
                ProductVM productVM = new ProductVM()
                {
                    ProductDTO = JsonConvert.DeserializeObject<ProductDTO>(Convert.ToString(productResponse.Result)),
                    ManufacturerList = JsonConvert.DeserializeObject<List<ManufacturerDTO>>(Convert.ToString(manufacturerResponse.Result))
                    .Select(i => new SelectListItem
                    {
                        Text = i.Name,
                        Value = i.Id.ToString()
                    }),
                    SubcategoryList = JsonConvert.DeserializeObject<List<SubcategoryDTO>>(Convert.ToString(subcategoryResponse.Result))
                    .Select(i => new SelectListItem
                    {
                        Text = i.Name,
                        Value = i.Id.ToString()
                    })
                };
                return View(productVM);
            }
            return RedirectToAction("Index");
        }


        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ProductVM productVM)
        {
            if (ModelState.IsValid)
            {
                APIResponse? productResponse = await _productService.UpdateAsync<APIResponse>(productVM.ProductDTO, HttpContext.Session.GetString(SD.SessionToken));
                if (productResponse != null && productResponse.isSuccess)
                {
                    TempData["success"] = "Product updated successfully";
                    return RedirectToAction("Index");
                }
                if (productResponse.ErrorMessages.Count > 0)
                {
                    foreach (var error in productResponse.ErrorMessages)
                    {
                        ModelState.AddModelError("ErrorMessages", error);
                    }
                }
            }

            APIResponse? manufacturerResponse = await _manufacturerService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SessionToken));
            APIResponse? subcategoryResponse = await _subcategoryService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SessionToken));
            if (manufacturerResponse != null && manufacturerResponse.isSuccess
                 && subcategoryResponse != null && subcategoryResponse.isSuccess)
            {
                productVM.ManufacturerList = JsonConvert.DeserializeObject<List<ManufacturerDTO>>(Convert.ToString(manufacturerResponse.Result))
                    .Select(i => new SelectListItem
                    {
                        Text = i.Name,
                        Value = i.Id.ToString()
                    });
                productVM.SubcategoryList = JsonConvert.DeserializeObject<List<SubcategoryDTO>>(Convert.ToString(subcategoryResponse.Result))
                    .Select(i => new SelectListItem
                    {
                        Text = i.Name,
                        Value = i.Id.ToString()
                    });
            }
            return View(productVM);
        }


        //GET
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _productService.GetAsync<APIResponse>(id, HttpContext.Session.GetString(SD.SessionToken));
            if (response != null && response.isSuccess)
            {
                ProductDTO productDTO = JsonConvert.DeserializeObject<ProductDTO>(Convert.ToString(response.Result));
                return View(productDTO);
            }
            return NotFound();
        }


        //POST
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeletePOST(ProductDTO productDTO)
        {
            var response = await _productService.DeleteAsync<APIResponse>(productDTO.Id, HttpContext.Session.GetString(SD.SessionToken));
            if (response != null && response.isSuccess)
            {
                TempData["success"] = "Product deleted successfully";
                return RedirectToAction("Index");
            }
            return NotFound();
        }
    }
}
