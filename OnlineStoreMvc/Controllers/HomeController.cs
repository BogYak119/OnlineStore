using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using OnlineStore.Models.DTO;
using OnlineStore.Models;
using OnlineStoreMvc.Services.IServices;
using AutoMapper;

namespace OnlineStoreMvc.Controllers
{
    public class HomeController : Controller
    {
        IProductService _productService;
        IMapper _mapper;

        public HomeController(IProductService productService, IMapper mapper)
        {
            _productService = productService;
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
    }
}
