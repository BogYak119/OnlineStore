using AutoMapper;
using Azure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using OnlineStore.DataAccess.Repository.IRepository;
using OnlineStore.Models;
using OnlineStore.Models.DTO;
using System.Net;

namespace OnlineStoreAPI.Controllers
{
    [Route("api/CategoryAPI")]
    [ApiController]
    public class CategoryAPIController : ControllerBase
    {
        protected APIResponse _response;
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly IMapper _mapper;
        public CategoryAPIController(IRepositoryWrapper repositoryWrapper, IMapper mapper)
        {
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;
            _response = new APIResponse();
        }


        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<APIResponse>> GetCategories()
        {
            try
            {
                IEnumerable<Category> categoryList = await _repositoryWrapper.Category.GetAllAsync();
                _response.Result = _mapper.Map<List<CategoryDTO>>(categoryList);
                _response.StatusCode = HttpStatusCode.OK;
                _response.isSuccess = true;

                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.isSuccess = false;
                _response.ErrorMessages = new List<KeyValuePair<string, string>> { new KeyValuePair<string, string>("error", ex.ToString())};
            }
            return _response;
        }


        [HttpGet("{id:int}", Name = "GetCategory")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetCategory(int id)
        {
            try
            {
                if (id <= 0)
                {
                    _response.isSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.ErrorMessages = new List<KeyValuePair<string, string>>{ new KeyValuePair<string, string>("error", "id <= 0" )};
                    return BadRequest(_response);
                }

                Category category = await _repositoryWrapper.Category.GetAsync(c => c.Id == id);

                if (category == null)
                {
                    _response.isSuccess = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.ErrorMessages = new List<KeyValuePair<string, string>> { new KeyValuePair<string, string>("error","category not found")};
                    return NotFound(_response);
                }

                _response.Result = _mapper.Map<CategoryDTO>(category);
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


        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<APIResponse>> CreateCategory([FromBody] CategoryCreateDTO categoryCreateDTO)
        {
            try
            {
                if (categoryCreateDTO == null)
                {
                    _response.isSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.ErrorMessages = new List<KeyValuePair<string, string>> { new KeyValuePair<string, string>("error", "Category is null")};
                    return BadRequest(_response);
                }
                if (await _repositoryWrapper.Category.GetAsync(c => c.Name == categoryCreateDTO.Name) != null)
                {
                    _response.isSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.ErrorMessages = new List<KeyValuePair<string, string>> { new KeyValuePair<string, string>("Name", "Category with the same name already exists")};
                    return BadRequest(_response);
                }

                Category category = _mapper.Map<Category>(categoryCreateDTO);

                await _repositoryWrapper.Category.CreateAsync(category);
                await _repositoryWrapper.SaveAsync();

                _response.isSuccess = true;
                _response.Result = _mapper.Map<CategoryDTO>(category);
                _response.StatusCode = HttpStatusCode.Created;

                return CreatedAtRoute("GetCategory", new { id = category.Id }, _response);
            }
            catch (Exception ex)
            {
                _response.isSuccess = false;
                _response.ErrorMessages = new List<KeyValuePair<string, string>> { new KeyValuePair<string, string>("error", ex.ToString()) };
            }
            return _response;
        }


        [HttpDelete("{id:int}", Name = "DeleteCategory")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> DeleteCategory(int id)
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

                Category category = await _repositoryWrapper.Category.GetAsync(c => c.Id == id);

                if (category == null)
                {
                    _response.isSuccess = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.ErrorMessages = new List<KeyValuePair<string, string>> { new KeyValuePair<string, string>("error","category not found") };
                    return NotFound(_response);
                }

                await _repositoryWrapper.Category.RemoveAsync(category);
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


        [HttpPut("{id:int}", Name = "UpdateCategory")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<APIResponse>> UpdateCategory(int id, [FromBody] CategoryDTO categoryDTO)
        {
            try
            {
                if (categoryDTO == null || id != categoryDTO.Id)
                {
                    _response.isSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.ErrorMessages = new List<KeyValuePair<string, string>> { new KeyValuePair<string, string>("error", "categoryDTO is null or id != categoryDTO.id")};
                    return BadRequest(_response);
                }
                if (await _repositoryWrapper.Category.GetAsync(c => c.Name == categoryDTO.Name) != null)
                {
                    _response.isSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.ErrorMessages = new List<KeyValuePair<string, string>> { new KeyValuePair<string, string>("Name", "Category with the same name already exists") };
                    return BadRequest(_response);
                }

                Category category = _mapper.Map<Category>(categoryDTO);
                await _repositoryWrapper.Category.UpdateAsync(category);

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
