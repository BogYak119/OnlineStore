using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using OnlineStore.Models.DTO;
using OnlineStore.Models;
using OnlineStore.Utility;
using OnlineStoreMvc.Services;
using OnlineStoreMvc.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using System.Data;
using Microsoft.AspNetCore.Identity;

namespace OnlineStoreMvc.Areas.Admin.Controllers
{
    [Authorize(Roles = "admin")]

    public class UserController : Controller
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public UserController(IUserService userService, IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }


        public async Task<IActionResult> Index()
        {
            List<UserDTO> userList = new List<UserDTO>();
            APIResponse? userResponse = await _userService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SessionToken));

            if (userResponse != null && userResponse.isSuccess)
            {
                userList = JsonConvert.DeserializeObject<List<UserDTO>>(Convert.ToString(userResponse.Result));
            }

            userList = userList.OrderBy(u => u.Role).ToList();

            return View(userList);
        }


        //GET
        public async Task<IActionResult> Delete(string id)
        {
            APIResponse? userResponse = await _userService.GetAsync<APIResponse>(id, HttpContext.Session.GetString(SD.SessionToken));
            if (userResponse != null && userResponse.isSuccess)
            {
                UserDTO userDTO = JsonConvert.DeserializeObject<UserDTO>(Convert.ToString(userResponse.Result));
                return View(userDTO);
            }
            return NotFound();
        }


        //POST
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeletePOST(UserDTO userDTO)
        {
            var userReponse = await _userService.DeleteAsync<APIResponse>(userDTO.ID, HttpContext.Session.GetString(SD.SessionToken));
            if (userReponse != null && userReponse.isSuccess)
            {
                TempData["success"] = "User deleted successfully";
                return RedirectToAction("Index");
            }
            return NotFound();
        }


        //GET
        public async Task<IActionResult> Edit(string id)
        {
            APIResponse? userResponse = await _userService.GetAsync<APIResponse>(id, HttpContext.Session.GetString(SD.SessionToken));
            if (userResponse != null && userResponse.isSuccess)
            {
                UserDTO userDTO = JsonConvert.DeserializeObject<UserDTO>(Convert.ToString(userResponse.Result));
                return View(userDTO);
            }
            return NotFound();
        }


        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UserDTO userDTO)
        {
            if (ModelState.IsValid)
            {
                var userResponse = await _userService.UpdateAsync<APIResponse>(userDTO, HttpContext.Session.GetString(SD.SessionToken));
                if (userResponse != null && userResponse.isSuccess)
                {
                    TempData["success"] = "User updated successfully";
                    return RedirectToAction("Index");
                }

                if (userResponse.ErrorMessages.Count > 0)
                {
                    foreach (var error in userResponse.ErrorMessages)
                    {
                        ModelState.AddModelError("ErrorMessages", error);
                    }
                }
                //foreach (var error in categoryResponse.ErrorMessages)
                //{
                //    ModelState.AddModelError(error.Key, error.Value);
                //}
            }
            return View(userDTO);
        }
    }
}
