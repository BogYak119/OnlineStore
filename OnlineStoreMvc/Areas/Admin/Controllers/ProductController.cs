using Microsoft.AspNetCore.Mvc;
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

namespace OnlineStoreMvc.Areas.Admin.Controllers
{
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
            var response = await _productService.GetAllAsync<APIResponse>();

            if (response != null && response.isSuccess)
            {
                productList = JsonConvert.DeserializeObject<List<ProductDTO>>(Convert.ToString(response.Result));
            }
            return View(productList);
        }


        //GET
        public async Task<IActionResult> Create()
        {
            var manufacturerResponse = await _manufacturerService.GetAllAsync<APIResponse>();
            var subcategoryResponse = await _subcategoryService.GetAllAsync<APIResponse>();
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
                APIResponse? productResponse = await _productService.CreateAsync<APIResponse>(_mapper.Map<ProductCreateDTO>(productVM.ProductDTO));
                if (productResponse != null && productResponse.isSuccess)
                {
                    TempData["success"] = "Product created successfully";
                    return RedirectToAction("Index");
                }
                foreach (var error in productResponse.ErrorMessages)
                {
                    ModelState.AddModelError(error.Key, error.Value);
                }
            }

            APIResponse? manufacturerResponse = await _manufacturerService.GetAllAsync<APIResponse>();
            APIResponse? subcategoryResponse = await _subcategoryService.GetAllAsync<APIResponse>();
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
            };
            return NotFound();
        }


        //GET
        public async Task<IActionResult> Edit(int id)
        {
            APIResponse? productResponse = await _productService.GetAsync<APIResponse>(id);
            APIResponse? manufacturerResponse = await _manufacturerService.GetAllAsync<APIResponse>();
            APIResponse? subcategoryResponse = await _subcategoryService.GetAllAsync<APIResponse>();
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
            return NotFound();

        }


        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ProductVM productVM)
        {
            if (ModelState.IsValid)
            {
                var response1 = await _productService.UpdateAsync<APIResponse>(productVM.ProductDTO);
                if (response1 != null && response1.isSuccess)
                {
                    TempData["success"] = "Product updated successfully";
                    return RedirectToAction("Index");
                }
                foreach (var error in response1.ErrorMessages)
                {
                    ModelState.AddModelError(error.Key, error.Value);
                }
            }

            APIResponse? manufacturerResponse = await _manufacturerService.GetAllAsync<APIResponse>();
            APIResponse? subcategoryResponse = await _subcategoryService.GetAllAsync<APIResponse>();
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
            return NotFound();
        }


        //GET
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _productService.GetAsync<APIResponse>(id);
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
            var response = await _productService.DeleteAsync<APIResponse>(productDTO.Id);
            if (response != null && response.isSuccess)
            {
                TempData["success"] = "Product deleted successfully";
                return RedirectToAction("Index");
            }
            return NotFound();
        }       
    }
}
