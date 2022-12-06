using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using OnlineStore.Models;
using OnlineStore.Models.DTO;
using OnlineStore.Models.ViewModels;
using OnlineStore.Utility;
using OnlineStoreMvc.Services.IServices;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace OnlineStoreMvc.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }
    

        [HttpGet]
        public IActionResult Login()
        {
            LoginRequestDTO loginRequest = new LoginRequestDTO();
            return View(loginRequest);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginRequestDTO loginRequest)
        {
            APIResponse response = await _authService.LoginAsync<APIResponse>(loginRequest);
            if(response != null && response.isSuccess)
            {
                LoginResponseDTO loginResponse = JsonConvert.DeserializeObject<LoginResponseDTO>(Convert.ToString(response.Result));

                var handler = new JwtSecurityTokenHandler();
                var jwt = handler.ReadJwtToken(loginResponse.Token);

                var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
                identity.AddClaim(new Claim(ClaimTypes.Name, jwt.Claims.FirstOrDefault(c => c.Type == "unique_name").Value));
                identity.AddClaim(new Claim(ClaimTypes.Role, jwt.Claims.FirstOrDefault(c => c.Type == "role").Value));
                var principal = new ClaimsPrincipal(identity);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);   

                HttpContext.Session.SetString(SD.SessionToken, loginResponse.Token);
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ModelState.AddModelError("Error", response.ErrorMessages.FirstOrDefault());
                return View(loginRequest);
            }
        }


        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegistrationRequestDTO registrationRequest)
        {
            APIResponse registrationResponse = await _authService.RegisterAync<APIResponse>(registrationRequest);
            if (registrationResponse != null && registrationResponse.isSuccess)
            {
                TempData["success"] = "Registration succeeded";
                return RedirectToAction("Login");
            }
            if (registrationResponse.ErrorMessages.Count > 0)
            {
                foreach (var error in registrationResponse.ErrorMessages)
                {
                    ModelState.AddModelError("ErrorMessages", error);
                }
            }
            return View();
        }


        [Authorize(Roles = "admin")]
        [HttpGet]
        public IActionResult AdminRegister()
        {
            RegisterVM registerVM = new RegisterVM()
            {
                RegistrationRequestDTO = new RegistrationRequestDTO()
                {
                    Role = ""
                },
                RoleList = SD.roles.ConvertAll(a =>
                {
                    return new SelectListItem()
                    {
                        Text = a.ToString(),
                        Value = a.ToString(),
                    };
                })
            };

            return View(registerVM);
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AdminRegister(RegisterVM registerVM)
        {
            if (ModelState.IsValid)
            {
                APIResponse registrationResponse = await _authService.RegisterAync<APIResponse>(registerVM.RegistrationRequestDTO, HttpContext.Session.GetString(SD.SessionToken));
                if (registrationResponse != null && registrationResponse.isSuccess)
                {
                    TempData["success"] = "User registered successfully";
                    return RedirectToAction("AdminRegister");
                }
                if (registrationResponse.ErrorMessages.Count > 0)
                {
                    foreach (var error in registrationResponse.ErrorMessages)
                    {
                        ModelState.AddModelError("ErrorMessages", error);
                    }
                }
            }
            registerVM.RoleList = SD.roles.ConvertAll(a =>
            {
                return new SelectListItem()
                {
                    Text = a.ToString(),
                    Value = a.ToString(),
                    Selected = false
                };
            });
            return View(registerVM);
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            HttpContext.Session.SetString(SD.SessionToken, "");
            return RedirectToAction("Index", "Home", new { area = ""});
        }


        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
