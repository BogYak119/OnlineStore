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
            var response = await _manufacturerService.GetAllAsync<APIResponse>();

            if (response != null && response.isSuccess)
            {
                manufacturerList = JsonConvert.DeserializeObject<List<ManufacturerDTO>>(Convert.ToString(response.Result));
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
                var response = await _manufacturerService.CreateAsync<APIResponse>(manufacturerCreateDTO);
                if (response != null && response.isSuccess)
                {
                    TempData["success"] = "Manufacturer created successfully";
                    return RedirectToAction("Index");
                }

                foreach (var error in response.ErrorMessages)
                {
                    ModelState.AddModelError(error.Key, error.Value);
                }
            }
            return View(manufacturerCreateDTO);
        }

        //GET
        public async Task<IActionResult> Edit(int id)
        {
            var response = await _manufacturerService.GetAsync<APIResponse>(id);
            if (response != null && response.isSuccess)
            {
                ManufacturerDTO manufacturerDTO = JsonConvert.DeserializeObject<ManufacturerDTO>(Convert.ToString(response.Result));
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
                var response = await _manufacturerService.UpdateAsync<APIResponse>(manufacturerDTO);
                if (response != null && response.isSuccess)
                {
                    TempData["success"] = "Manufacturer updated successfully";
                    return RedirectToAction("Index");
                }

                foreach (var error in response.ErrorMessages)
                {
                    ModelState.AddModelError(error.Key, error.Value);
                }
            }
            return View(manufacturerDTO);
        }

        //GET
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _manufacturerService.GetAsync<APIResponse>(id);
            if (response != null && response.isSuccess)
            {
                ManufacturerDTO manufacturerDTO = JsonConvert.DeserializeObject<ManufacturerDTO>(Convert.ToString(response.Result));
                return View(manufacturerDTO);
            }
            return NotFound();
        }

        //POST
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeletePOST(ManufacturerDTO manufacturerDTO)
        {
            var response = await _manufacturerService.DeleteAsync<APIResponse>(manufacturerDTO.Id);
            if (response != null && response.isSuccess)
            {
                TempData["success"] = "Manufacturer deleted successfully";
                return RedirectToAction("Index");
            }
            return NotFound();
        }
    }
}
