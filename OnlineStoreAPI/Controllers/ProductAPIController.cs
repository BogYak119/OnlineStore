using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OnlineStore.DataAccess.Repository.IRepository;
using OnlineStore.Models.DTO;
using OnlineStore.Models;
using System.Net;

namespace OnlineStoreAPI.Controllers
{
    [Route("api/ProductAPI")]
    [ApiController]
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
                _response.isSuccess = true;
                IEnumerable<Product> productList = await _repositoryWrapper.Product.GetAllAsync();
                _response.Result = _mapper.Map<List<ProductDTO>>(productList);
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
                    _response.isSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.ErrorMessages = new List<KeyValuePair<string, string>> { new KeyValuePair<string, string>("error", "id <= 0") };
                    return BadRequest(_response);
                }

                Product product = await _repositoryWrapper.Product.GetAsync(p => p.Id == id);

                if (product == null)
                {
                    _response.isSuccess = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.ErrorMessages = new List<KeyValuePair<string, string>> { new KeyValuePair<string, string>("error", "Product not found") };
                    return NotFound(_response);
                }

                _response.isSuccess = true;
                _response.Result = _mapper.Map<ProductDTO>(product);
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
        public async Task<ActionResult<APIResponse>> CreateProduct([FromBody] ProductCreateDTO productCreateDTO)
        {
            try
            {
                if (productCreateDTO == null)
                {
                    _response.isSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.ErrorMessages = new List<KeyValuePair<string, string>> { new KeyValuePair<string, string>("error", "Product is null") };
                    return BadRequest(_response);
                }
                if (await _repositoryWrapper.Subcategory.GetAsync(sc => sc.Id == productCreateDTO.SubcategoryId) == null)
                {
                    _response.isSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    new List<KeyValuePair<string, string>> { new KeyValuePair<string, string>("SubcategoryName", "Subcategory ID is not valid") };
                    return BadRequest(_response);
                }
                if (await _repositoryWrapper.Manufacturer.GetAsync(m => m.Id == productCreateDTO.ManufacturerId) == null)
                {
                    _response.isSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    new List<KeyValuePair<string, string>> { new KeyValuePair<string, string>("ManufacturerName", "Manufacturer ID is not valid") };
                    return BadRequest(_response);
                }               
                //if (await _repositoryWrapper.Product.GetAsync(p => p.Id == productCreateDTO.SubcategoryId) == null)
                //{
                //    _response.isSuccess = false;
                //    _response.StatusCode = HttpStatusCode.BadRequest;
                //    _response.ErorrMessages = new List<string> { "Product ID is not valid" };
                //    return BadRequest(_response);
                //}
                if (await _repositoryWrapper.Product.GetAsync(p => p.Name == productCreateDTO.Name) != null
                   /* && await _repositoryWrapper.Subcategory.GetAsync(c => c.CategoryId == subcategoryCreateDTO.CategoryId) != null*/)
                {
                    _response.isSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.ErrorMessages = new List<KeyValuePair<string, string>> { new KeyValuePair<string, string>("ProductDTO.Name", "Product with the same name already exists") };
                    return BadRequest(_response);
                }

                Product product = _mapper.Map<Product>(productCreateDTO);
                //product.CategoryId = product.Subcategory.CategoryId;

                await _repositoryWrapper.Product.CreateAsync(product);
                await _repositoryWrapper.SaveAsync();

                _response.Result = _mapper.Map<ProductDTO>(product);
                _response.StatusCode = HttpStatusCode.Created;
                _response.isSuccess = true;

                return CreatedAtRoute("GetProduct", new { id = product.Id }, _response);
            }
            catch (Exception ex)
            {
                _response.isSuccess = false;
                _response.ErrorMessages = new List<KeyValuePair<string, string>> { new KeyValuePair<string, string>("error", ex.ToString()) };
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
                Product product = await _repositoryWrapper.Product.GetAsync(p => p.Id == id);

                if (product == null)
                {
                    _response.isSuccess = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.ErrorMessages = new List<KeyValuePair<string, string>> { new KeyValuePair<string, string>("error", "Product not found") };
                    return NotFound(_response);
                }

                await _repositoryWrapper.Product.RemoveAsync(product);
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


        [HttpPut("{id:int}", Name = "UpdateProduct")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<APIResponse>> UpdateProduct(int id, [FromBody] ProductDTO productDTO)
        {
            try
            {
                if (productDTO == null || id != productDTO.Id)
                {
                    _response.isSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.ErrorMessages = new List<KeyValuePair<string, string>> { new KeyValuePair<string, string>("error", "productDTO is null or id != productDTO.id") };
                    return BadRequest(_response);
                }
                if (id <= 0)
                {
                    _response.isSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    new List<KeyValuePair<string, string>> { new KeyValuePair<string, string>("ProductDTO.Name", "Product ID is not valid") };
                    return BadRequest(_response);
                }
                if (await _repositoryWrapper.Subcategory.GetAsync(sc => sc.Id == productDTO.SubcategoryId) == null)
                {
                    _response.isSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    new List<KeyValuePair<string, string>> { new KeyValuePair<string, string>("SubcategoryName", "Subcategory ID is not valid") };
                    return BadRequest(_response);
                }
                if (await _repositoryWrapper.Manufacturer.GetAsync(m => m.Id == productDTO.ManufacturerId) == null)
                {
                    _response.isSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    new List<KeyValuePair<string, string>> { new KeyValuePair<string, string>("ManufacturerName", "Manufacturer ID is not valid") };
                    return BadRequest(_response);
                }
                if (await _repositoryWrapper.Product.GetAsync(p => p.Name == productDTO.Name) != null)
                {
                    _response.isSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.ErrorMessages = new List<KeyValuePair<string, string>> { new KeyValuePair<string, string>("ProductDTO.Name", "Product with the same name already exists") };
                    return BadRequest(_response);
                }

                Product product = _mapper.Map<Product>(productDTO);
                await _repositoryWrapper.Product.UpdateAsync(product);

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
