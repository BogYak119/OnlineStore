using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OnlineStore.DataAccess.Repository;
using OnlineStore.DataAccess.Repository.IRepository;
using OnlineStore.Models;
using OnlineStore.Models.DTO;
using System.Net;

namespace OnlineStoreAPI.Controllers
{
    [Route("api/UserAPI")]
    [ApiController]
    public class UserAPIController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        protected APIResponse _response; 

        public UserAPIController(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;   
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
                ModelState.AddModelError("ErrorMessages", "User with this name already exists");

                return BadRequest(ModelState);
            }

            RegistrationResponseDTO? registrationResponseDTO = await _userRepository.Register(registerRequstDTO);

            if(registrationResponseDTO == null)
            {
                _response.isSuccess = false;
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.ErrorMessages.Add("An error occured during registration");
                return BadRequest(_response);
            }
            if(registrationResponseDTO.userDTO == null)
            {
                _response.isSuccess = false;
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.ErrorMessages = registrationResponseDTO.Errors;
                return BadRequest(_response);
            }

            _response.isSuccess = true;
            _response.StatusCode = HttpStatusCode.OK;
            return Ok(_response);
        }


        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<APIResponse>> GetUsers()
        {
            try
            {
                IEnumerable<UserDTO> userList = await _userRepository.GetAllAsync();
                _response.Result = userList;
                _response.StatusCode = HttpStatusCode.OK;

                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.isSuccess = false;
                _response.ErrorMessages.Add(ex.Message);
            }
            return _response;
        }


        [HttpGet("{id:Guid}", Name = "GetUser")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize(Roles = "admin")]

        public async Task<ActionResult<APIResponse>> GetUser(string id)
        {
            try
            {
                var user = await _userRepository.GetAsync(u => u.Id == id);

                if (user == null)
                {
                    return NotFound(_response);
                }

                _response.Result = user;
                _response.StatusCode = HttpStatusCode.OK;

                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.isSuccess = false;
                _response.ErrorMessages.Add(ex.Message);
            }
            return _response;
        }


        [HttpDelete("{id:Guid}", Name = "DeleteUser")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<APIResponse>> DeleteUser(string id)
        {
            try
            {
                var result = await _userRepository.RemoveAsync(id);

                if(result.Count > 0)
                {
                    _response.isSuccess = false;
                    foreach (var error in result)
                    {
                        _response.ErrorMessages.Add(error);
                    }
                    return (_response);
                }              
                _response.StatusCode = HttpStatusCode.NoContent;

                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.isSuccess = false;
                _response.ErrorMessages.Add(ex.Message);
            }
            return _response;
        }


        [HttpPut("{id:Guid}", Name = "UpdateUser")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<APIResponse>> UpdateUser(string id, [FromBody] UserDTO userDTO)
        {
            try
            {
                if (userDTO == null || id != userDTO.ID)
                {
                    return BadRequest(_response);
                }
                if (await _userRepository.GetAsync(u => u.UserName == userDTO.UserName) != null
                    && _userRepository.GetAsync(u => u.Id == userDTO.ID).Result.UserName != userDTO.UserName)
                {
                    ModelState.AddModelError("ErrorMessages", "User already exists");
                    return BadRequest(ModelState);
                }

                await _userRepository.UpdateAsync(userDTO);

                _response.StatusCode = HttpStatusCode.NoContent;

                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.isSuccess = false;
                _response.ErrorMessages.Add(ex.Message);
            }
            return _response;
        }
    }
}
