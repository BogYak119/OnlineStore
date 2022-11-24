using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using OnlineStore.DataAccess.Repository.IRepository;
using OnlineStore.Models;
using OnlineStore.Models.DTO;
using OnlineStoreMvc.Services.IServices;

namespace OnlineStoreMvc.Areas.Admin.Controllers
{
    public class ManufacturerController : Controller
    {
        private readonly IManufacturerService _manufacturerService;
        private readonly IMapper _mapper;

        public ManufacturerController(IManufacturerService manufacturerService, IMapper mapper)
        {
            _manufacturerService = manufacturerService;
            _mapper = mapper;
        }


        public async Task<IActionResult> Index()
        {
            List<ManufacturerDTO> manufacturerList = new List<ManufacturerDTO>();
            APIResponse? manufacturerResponse = await _manufacturerService.GetAllAsync<APIResponse>();

            if (manufacturerResponse != null && manufacturerResponse.isSuccess)
            {
                manufacturerList = JsonConvert.DeserializeObject<List<ManufacturerDTO>>(Convert.ToString(manufacturerResponse.Result));
            }
            return View(manufacturerList);
        }


        //GET
        public IActionResult Create()
        {
            return View();
        }


        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ManufacturerCreateDTO manufacturerCreateDTO)
        {
            if (ModelState.IsValid)
            {
                APIResponse? manufacturerResponse = await _manufacturerService.CreateAsync<APIResponse>(manufacturerCreateDTO);
                if (manufacturerResponse != null && manufacturerResponse.isSuccess)
                {
                    TempData["success"] = "Manufacturer created successfully";
                    return RedirectToAction("Index");
                }

                foreach (var error in manufacturerResponse.ErrorMessages)
                {
                    ModelState.AddModelError(error.Key, error.Value);
                }
            }
            return View(manufacturerCreateDTO);
        }

 
        //GET
        public async Task<IActionResult> Edit(int id)
        {
            APIResponse? manufacturerResponse = await _manufacturerService.GetAsync<APIResponse>(id);
            if (manufacturerResponse != null && manufacturerResponse.isSuccess)
            {
                ManufacturerDTO manufacturerDTO = JsonConvert.DeserializeObject<ManufacturerDTO>(Convert.ToString(manufacturerResponse.Result));
                return View(manufacturerDTO);
            }
            return NotFound();
        }


        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ManufacturerDTO manufacturerDTO)
        {
            if (ModelState.IsValid)
            {
                APIResponse? manufacturerResponse = await _manufacturerService.UpdateAsync<APIResponse>(manufacturerDTO);
                if (manufacturerResponse != null && manufacturerResponse.isSuccess)
                {
                    TempData["success"] = "Manufacturer updated successfully";
                    return RedirectToAction("Index");
                }

                foreach (var error in manufacturerResponse.ErrorMessages)
                {
                    ModelState.AddModelError(error.Key, error.Value);
                }
            }
            return View(manufacturerDTO);
        }


        //GET
        public async Task<IActionResult> Delete(int id)
        {
            APIResponse? manufacturerResponse = await _manufacturerService.GetAsync<APIResponse>(id);
            if (manufacturerResponse != null && manufacturerResponse.isSuccess)
            {
                ManufacturerDTO manufacturerDTO = JsonConvert.DeserializeObject<ManufacturerDTO>(Convert.ToString(manufacturerResponse.Result));
                return View(manufacturerDTO);
            }
            return NotFound();
        }


        //POST
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeletePOST(ManufacturerDTO manufacturerDTO)
        {
            APIResponse? manufacturerResponse = await _manufacturerService.DeleteAsync<APIResponse>(manufacturerDTO.Id);
            if (manufacturerResponse != null && manufacturerResponse.isSuccess)
            {
                TempData["success"] = "Manufacturer deleted successfully";
                return RedirectToAction("Index");
            }
            return NotFound();
        }
    }
}
