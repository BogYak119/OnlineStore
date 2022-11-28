using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OnlineStore.DataAccess.Repository.IRepository;
using OnlineStore.Models;
using OnlineStore.Models.DTO;
using System.Net;

namespace OnlineStoreAPI.Controllers
{
    [Route("api/UserAuth")]
    [ApiController]
    public class UserAPIController : Controller
    {
        private readonly IUserRepository _userRepository;
        protected APIResponse _response; 

        public UserAPIController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
            _response = new APIResponse();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO loginRequstDTO)
        {
            LoginResponseDTO? loginResponse = await _userRepository.Login(loginRequstDTO);
            if(loginResponse.User == null || string.IsNullOrEmpty(loginResponse.Token)) 
            {
                _response.isSuccess = false;
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.ErrorMessages.Add("Username or password is invalid");
                return BadRequest(_response);
            }
            _response.isSuccess = true;
            _response.StatusCode = HttpStatusCode.OK;
            _response.Result = loginResponse;   
            return Ok(_response);
        }


        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegistrationRequestDTO registerRequstDTO)
        {
            bool isUserNameUnique = _userRepository.isUniqueUser(registerRequstDTO.UserName);

            if(isUserNameUnique == false)
            {
                _response.isSuccess = false;
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.ErrorMessages.Add("Username already exists");
                return BadRequest(_response);
            }

            LocalUser? user = await _userRepository.Register(registerRequstDTO);

            if(user == null)
            {
                _response.isSuccess = false;
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.ErrorMessages.Add("An error occured during registration");
                return BadRequest(_response);
            }

            _response.isSuccess = true;
            _response.StatusCode = HttpStatusCode.OK;
            return Ok(_response);
        }
    }
}
