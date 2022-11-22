using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OnlineStore.DataAccess.Repository.IRepository;
using OnlineStore.Models;
using OnlineStore.Models.DTO;
using System.Net;

namespace OnlineStoreAPI.Controllers
{
    [Route("api/ManufacturerAPIController")]
    [ApiController]
    public class ManufacturerAPIController : ControllerBase
    {
        protected APIResponse _response;
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly IMapper _mapper;
        public ManufacturerAPIController(IRepositoryWrapper repositoryWrapper, IMapper mapper)
        {
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;
            _response = new APIResponse();
        }


        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<APIResponse>> GetManufacturers()
        {
            try
            {
                IEnumerable<Manufacturer> manufacturerList = await _repositoryWrapper.Manufacturer.GetAllAsync();
                _response.Result = _mapper.Map<List<ManufacturerDTO>>(manufacturerList);
                _response.StatusCode = HttpStatusCode.OK;
                _response.isSuccess = true;

                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.isSuccess = false;
                _response.ErorrMessages = new List<string> { ex.ToString() };
            }
            return _response;
        }


        [HttpGet("{id:int}", Name = "GetManufacturer")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetManufacturer(int id)
        {
            try
            {
                if (id <= 0)
                {
                    _response.isSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.ErorrMessages = new List<string> { "id <= 0" };
                    return BadRequest(_response);
                }

                Manufacturer manufacturer = await _repositoryWrapper.Manufacturer.GetAsync(m => m.Id == id);

                if (manufacturer == null)
                {
                    _response.isSuccess = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.ErorrMessages = new List<string> { "Manufacturer not found" };
                    return NotFound(_response);
                }

                _response.Result = _mapper.Map<ManufacturerDTO>(manufacturer);
                _response.StatusCode = HttpStatusCode.OK;
                _response.isSuccess = true;

                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.isSuccess = false;
                _response.ErorrMessages = new List<string> { ex.ToString() };
            }
            return _response;
        }


        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<APIResponse>> CreateManufacturer([FromBody] ManufacturerCreateDTO manufacturerCreateDTO)
        {
            try
            {
                if (manufacturerCreateDTO == null)
                {
                    _response.isSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.ErorrMessages = new List<string> { "Manufacturer is null" };
                    return BadRequest(_response);
                }
                if (await _repositoryWrapper.Manufacturer.GetAsync(m => m.Name == manufacturerCreateDTO.Name) != null)
                {
                    _response.isSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.ErorrMessages = new List<string> { "Manufacturer with the same name already exists" };
                    return BadRequest(_response);
                }

                Manufacturer manufacturer = _mapper.Map<Manufacturer>(manufacturerCreateDTO);

                await _repositoryWrapper.Manufacturer.CreateAsync(manufacturer);
                await _repositoryWrapper.SaveAsync();

                _response.Result = _mapper.Map<ManufacturerDTO>(manufacturer);
                _response.StatusCode = HttpStatusCode.Created;
                _response.isSuccess = true;

                return CreatedAtRoute("GetManufacturer", new { id = manufacturer.Id }, _response);
            }
            catch (Exception ex)
            {
                _response.isSuccess = false;
                _response.ErorrMessages = new List<string> { ex.ToString() };
            }
            return _response;
        }


        [HttpDelete("{id:int}", Name = "DeleteManufacturer")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> DeleteManufacturer(int id)
        {
            try
            {
                if (id <= 0)
                {
                    _response.isSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.ErorrMessages = new List<string> { "id <= 0" };
                    return BadRequest(_response);
                }

                Manufacturer manufacturer = await _repositoryWrapper.Manufacturer.GetAsync(m => m.Id == id);

                if (manufacturer == null)
                {
                    _response.isSuccess = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.ErorrMessages = new List<string> { "Manufacturer not found" };
                    return NotFound(_response);
                }

                await _repositoryWrapper.Manufacturer.RemoveAsync(manufacturer);
                await _repositoryWrapper.SaveAsync();

                _response.StatusCode = HttpStatusCode.NoContent;
                _response.isSuccess = true;

                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.isSuccess = false;
                _response.ErorrMessages = new List<string> { ex.ToString() };
            }
            return _response;
        }


        [HttpPut("{id:int}", Name = "UpdateManufacturer")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<APIResponse>> UpdateManufacturer(int id, [FromBody] ManufacturerDTO manufacturerDTO)
        {
            try
            {
                if (manufacturerDTO == null || id != manufacturerDTO.Id)
                {
                    _response.isSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.ErorrMessages = new List<string> { "manufacturerDTO is null or id != manufacturerDTO.id" };
                    return BadRequest(_response);
                }
                if (_repositoryWrapper.Manufacturer.GetAsync(m => m.Name == manufacturerDTO.Name, false).Result != null
                   && _repositoryWrapper.Manufacturer.GetAsync(m => m.Id == id, false).Result.Name != manufacturerDTO.Name)
                {
                    _response.isSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.ErorrMessages = new List<string> { "Manufacturer with the same name already exists" };
                    return BadRequest(_response);
                }

                Manufacturer manufacturer = _mapper.Map<Manufacturer>(manufacturerDTO);
                await _repositoryWrapper.Manufacturer.UpdateAsync(manufacturer);

                _response.isSuccess = true;
                _response.StatusCode = HttpStatusCode.NoContent;

                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.isSuccess = false;
                _response.ErorrMessages = new List<string> { ex.ToString() };
            }
            return _response;
        }
    }
}
