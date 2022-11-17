using AutoMapper;
using Azure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using OnlineStore.DataAccess.Repository.IRepository;
using OnlineStore.Models;
using OnlineStore.Models.DTO;

namespace OnlineStoreAPI.Controllers
{
    [Route("api/CategoryAPI")]
    [ApiController]
    public class CategoryAPIController : ControllerBase
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly IMapper _mapper;
        public CategoryAPIController(IRepositoryWrapper repositoryWrapper, IMapper mapper)
        {
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;
        }


        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<CategoryDTO>>> GetCategories()
        {
            IEnumerable<Category> categoryList = await _repositoryWrapper.Category.GetAllAsync();
            return Ok(_mapper.Map<List<CategoryDTO>>(categoryList));
        }


        [HttpGet("{id:int}", Name = "GetCategory")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<CategoryDTO>> GetCategory(int id)
        {
            if (id == 0)
                return BadRequest();

            Category category = await _repositoryWrapper.Category.GetAsync(c => c.Id == id);

            if (category == null)
                return NotFound();

            return Ok(_mapper.Map<CategoryDTO>(category));
        }


        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        //[ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<CategoryDTO>> CreateCategory([FromBody]CategoryCreateDTO categotyCreateDTO)
        {
            if (categotyCreateDTO == null)
                return BadRequest();

            /*if (categoryDTO.Id <= 0)
                return StatusCode(StatusCodes.Status500InternalServerError);*/

            if (await _repositoryWrapper.Category.GetAsync(c => c.Name == categotyCreateDTO.Name) != null)
            {
                ModelState.AddModelError("name", "Category with the same name already exists");
                return BadRequest(ModelState);
            }

            Category category = _mapper.Map<Category>(categotyCreateDTO);

            await _repositoryWrapper.Category.CreateAsync(category);
            await _repositoryWrapper.SaveAsync();

            return CreatedAtRoute("GetCategory", new { id = category.Id }, category);
        }


        [HttpDelete("{id:int}", Name = "DeleteCategory")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            if (id == 0)
                return BadRequest();

            Category category = await _repositoryWrapper.Category.GetAsync(c => c.Id == id);

            if (category == null)
                return NotFound();

            await _repositoryWrapper.Category.RemoveAsync(category);
            await _repositoryWrapper.SaveAsync();

            return NoContent();
        }


        [HttpPut("{id:int}", Name = "UpdateCategory")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateCategory(int id, [FromBody]CategoryDTO categoryDTO)
        {
            if (categoryDTO == null || id != categoryDTO.Id)
                return BadRequest();

            Category category = _mapper.Map<Category>(categoryDTO);

            await _repositoryWrapper.Category.UpdateAsync(category);
            await _repositoryWrapper.SaveAsync();

            return NoContent();
        }


        /*public IActionResult UpdatePartialCategory(int id, JsonPatchDocument<CategoryDTO> patchDTO)
        {
            if (patchDTO == null || id == 0)
                return BadRequest();

            Category category = _repositoryWrapper.Category.GetFirstOrDefault(c => c.Id == id);

            CategoryDTO categoryDTO = new()
            {
                Id = category.Id,
                Name = category.Name,
            };

            if (category == null)
                return BadRequest();

            patchDTO.ApplyTo(categoryDTO, ModelState);

            category = new()
            {
                Id = categoryDTO.Id,
                Name = categoryDTO.Name,
                UpdatedDate = DateTime.Now
            };
            _repositoryWrapper.Category.Update(category);
            _repositoryWrapper.Save();

            if (!ModelState.IsValid)
                return BadRequest();

            return NoContent();
        }*/
    }
}
