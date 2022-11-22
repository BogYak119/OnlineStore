using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OnlineStore.DataAccess.Repository.IRepository;
using OnlineStore.Models.DTO;
using OnlineStore.Models;
using System.Net;

namespace OnlineStoreAPI.Controllers
{
    [Route("api/SubcategoryAPIController")]
    [ApiController]
    public class SubcategoryAPIController : ControllerBase
    {
        protected APIResponse _response;
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly IMapper _mapper;
        public SubcategoryAPIController(IRepositoryWrapper repositoryWrapper, IMapper mapper)
        {
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;
            _response = new APIResponse();
        }


        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<APIResponse>> GetSubcategories()
        {
            try
            {
                IEnumerable<Subcategory> subcategoryList = await _repositoryWrapper.Subcategory.GetAllAsync();
                _response.Result = _mapper.Map<List<SubcategoryDTO>>(subcategoryList);
                _response.StatusCode = HttpStatusCode.OK;
                _response.isSuccess = true;

                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.isSuccess = false;
                _response.ErrorMessages = new List<KeyValuePair<string, string>> { new KeyValuePair<string, string>("error", ex.ToString()) };
            }
            return _response;
        }


        [HttpGet("{id:int}", Name = "GetSubcategory")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetSubcategory(int id)
        {
            try
            {
                if (id <= 0)
                {
                    _response.isSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.ErrorMessages = new List<KeyValuePair<string, string>> { new KeyValuePair<string, string>("error", "id <= 0") };
                    return BadRequest(_response);
                }

                Subcategory subcategory = await _repositoryWrapper.Subcategory.GetAsync(sc => sc.Id == id);

                if (subcategory == null)
                {
                    _response.isSuccess = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.ErrorMessages = new List<KeyValuePair<string, string>> { new KeyValuePair<string, string>("error", "Subcategory not found") };
                    return NotFound(_response);
                }

                _response.isSuccess = true;
                _response.Result = _mapper.Map<SubcategoryDTO>(subcategory);
                _response.StatusCode = HttpStatusCode.OK;

                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.isSuccess = false;
                _response.ErrorMessages = new List<KeyValuePair<string, string>> { new KeyValuePair<string, string>("error", ex.ToString()) };
            }
            return _response;
        }


        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<APIResponse>> CreateSubcategory([FromBody] SubcategoryCreateDTO subcategoryCreateDTO)
        {
            try
            {
                if (subcategoryCreateDTO == null)
                {
                    _response.isSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.ErrorMessages = new List<KeyValuePair<string, string>> { new KeyValuePair<string, string>("error", "Subcategory is null") };
                    return BadRequest(_response);
                }
                if (await _repositoryWrapper.Category.GetAsync(c => c.Id == subcategoryCreateDTO.CategoryId) == null)
                {
                    _response.isSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    new List<KeyValuePair<string, string>> { new KeyValuePair<string, string>("error", "Category ID is not valid")};
                    return BadRequest(_response);
                }
                if (await _repositoryWrapper.Subcategory.GetAsync(c => c.Name == subcategoryCreateDTO.Name) != null
                    && await _repositoryWrapper.Subcategory.GetAsync(c => c.CategoryId == subcategoryCreateDTO.CategoryId) != null)
                {
                    _response.isSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.ErrorMessages = new List<KeyValuePair<string, string>> { new KeyValuePair<string, string>("SubcategoryName", "Subcategory with the same name already exists") };
                    return BadRequest(_response);
                }

                Subcategory subcategory = _mapper.Map<Subcategory>(subcategoryCreateDTO);

                await _repositoryWrapper.Subcategory.CreateAsync(subcategory);
                await _repositoryWrapper.SaveAsync();

                _response.Result = _mapper.Map<SubcategoryDTO>(subcategory);
                _response.StatusCode = HttpStatusCode.Created;
                _response.isSuccess = true;

                return CreatedAtRoute("GetSubcategory", new { id = subcategory.Id }, _response);
            }
            catch (Exception ex)
            {
                _response.isSuccess = false;
                _response.ErrorMessages = new List<KeyValuePair<string, string>> { new KeyValuePair<string, string>("error", ex.ToString()) };
            }
            return _response;
        }


        [HttpDelete("{id:int}", Name = "DeleteSubcategory")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> DeleteSubcategory(int id)
        {
            try
            {
                if (id <= 0)
                {
                    _response.isSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.ErrorMessages = new List<KeyValuePair<string, string>> { new KeyValuePair<string, string>("error", "id <= 0") };
                    return BadRequest(_response);
                }

                Subcategory subcategory = await _repositoryWrapper.Subcategory.GetAsync(sc => sc.Id == id);

                if (subcategory == null)
                {
                    _response.isSuccess = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.ErrorMessages = new List<KeyValuePair<string, string>> { new KeyValuePair<string, string>("error", "Subcategory not found") };
                    return NotFound(_response);
                }

                await _repositoryWrapper.Subcategory.RemoveAsync(subcategory);
                await _repositoryWrapper.SaveAsync();

                _response.StatusCode = HttpStatusCode.NoContent;
                _response.isSuccess = true;

                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.isSuccess = false;
                _response.ErrorMessages = new List<KeyValuePair<string, string>> { new KeyValuePair<string, string>("error", ex.ToString()) };
            }
            return _response;
        }


        [HttpPut("{id:int}", Name = "UpdateSubcategory")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<APIResponse>> UpdateSubcategory(int id, [FromBody] SubcategoryDTO subcategoryDTO)
        {
            try
            {
                if (subcategoryDTO == null || id != subcategoryDTO.Id)
                {
                    _response.isSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.ErrorMessages = new List<KeyValuePair<string, string>> { new KeyValuePair<string, string>("error", "subcategoryDTO is null or id != sbucategoryDTO.id") };
                    return BadRequest(_response);
                }
                if (await _repositoryWrapper.Subcategory.GetAsync(sc => sc.Name == 
                subcategoryDTO.Name && sc.CategoryId == subcategoryDTO.CategoryId) != null)
                {
                    _response.isSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.ErrorMessages = new List<KeyValuePair<string, string>> { new KeyValuePair<string, string>("SubcategoryName", "Subcategory with the same name already exists") };
                    return BadRequest(_response);
                }

                Subcategory subcategory = _mapper.Map<Subcategory>(subcategoryDTO);
                await _repositoryWrapper.Subcategory.UpdateAsync(subcategory);

                _response.StatusCode = HttpStatusCode.NoContent;
                _response.isSuccess = true;

                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.isSuccess = false;
                _response.ErrorMessages = new List<KeyValuePair<string, string>> { new KeyValuePair<string, string>("error", ex.ToString()) };
            }
            return _response;
        }
    }
}
