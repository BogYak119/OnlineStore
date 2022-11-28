using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OnlineStore.DataAccess.Repository.IRepository;
using OnlineStore.Models.DTO;
using OnlineStore.Models;
using System.Net;
using Microsoft.AspNetCore.Authorization;
using System.Data;

namespace OnlineStoreAPI.Controllers
{
    [Route("api/ProductAPI")]
    [ApiController]
    [Authorize(Roles = "admin")]
    public class ProductAPIController : ControllerBase
    {
        protected APIResponse _response;
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly IMapper _mapper;
        public ProductAPIController(IRepositoryWrapper repositoryWrapper, IMapper mapper)
        {
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;
            _response = new APIResponse();
        }


        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<APIResponse>> GetProducts()
        {
            try
            {
                IEnumerable<Product> productList = await _repositoryWrapper.Product.GetAllAsync();
                _response.Result = _mapper.Map<List<ProductDTO>>(productList);
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


        [HttpGet("{id:int}", Name = "GetProduct")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetProduct(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest(_response);
                }

                Product product = await _repositoryWrapper.Product.GetAsync(p => p.Id == id);

                if (product == null)
                {
                    return NotFound(_response);
                }

                _response.Result = _mapper.Map<ProductDTO>(product);
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


        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<APIResponse>> CreateProduct([FromBody] ProductCreateDTO productCreateDTO)
        {
            try
            {
                if (productCreateDTO == null)
                {
                    return BadRequest(_response);
                }
                if (await _repositoryWrapper.Subcategory.GetAsync(sc => sc.Id == productCreateDTO.SubcategoryId) == null)
                {
                    return BadRequest(_response);
                }
                if (await _repositoryWrapper.Manufacturer.GetAsync(m => m.Id == productCreateDTO.ManufacturerId) == null)
                {
                    return BadRequest(_response);
                }
                if (await _repositoryWrapper.Product.GetAsync(p => p.Name == productCreateDTO.Name) != null)
                {
                    ModelState.AddModelError("ErrorMessages", "Product already exists");
                    return BadRequest(ModelState);
                }

                Product product = _mapper.Map<Product>(productCreateDTO);

                await _repositoryWrapper.Product.CreateAsync(product);
                await _repositoryWrapper.SaveAsync();

                _response.Result = _mapper.Map<ProductDTO>(product);
                _response.StatusCode = HttpStatusCode.Created;

                return CreatedAtRoute("GetProduct", new { id = product.Id }, _response);
            }
            catch (Exception ex)
            {
                _response.isSuccess = false;
                _response.ErrorMessages.Add(ex.Message);
            }
            return _response;
        }


        [HttpDelete("{id:int}", Name = "DeleteProduct")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> DeleteProduct(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest(_response);
                }

                Product product = await _repositoryWrapper.Product.GetAsync(p => p.Id == id);

                if (product == null)
                {
                    return NotFound(_response);
                }

                await _repositoryWrapper.Product.RemoveAsync(product);
                await _repositoryWrapper.SaveAsync();

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


        [HttpPut("{id:int}", Name = "UpdateProduct")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<APIResponse>> UpdateProduct(int id, [FromBody] ProductDTO productDTO)
        {
            try
            {
                if (productDTO == null || id <= 0 || id != productDTO.Id)
                {
                    return BadRequest(_response);
                }

                if (await _repositoryWrapper.Product.GetAsync(p => p.Name == productDTO.Name && p.Id != productDTO.Id) != null)
                {
                    ModelState.AddModelError("ErrorMessages", "Product already exists");
                    return BadRequest(ModelState);
                }

                Product product = _mapper.Map<Product>(productDTO);
                await _repositoryWrapper.Product.UpdateAsync(product);

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
