//temporary unused


/*using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OnlineStore.Models;
using OnlineStore.Models.DTO;
using System.Data;
using System.Net;

namespace OnlineStoreAPI.Controllers
{
    [Route("api/FileAPIController")]
    [ApiController]
    public class FileAPIController : ControllerBase
    {
        protected APIResponse _response;
        private readonly IWebHostEnvironment _webHostEnvironment;   
        public FileAPIController(IWebHostEnvironment webHostEnvironment)
        {
            _response = new APIResponse();
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        //[Authorize(Roles = "admin")]
        public async Task<ActionResult<APIResponse>> PostImage(IFormFile image)
        {
            try
            {
                if (image == null)
                {
                    return BadRequest(_response);
                }
                string imageName = Guid.NewGuid().ToString();
                var uploads = @"Images\Products";
                var extension = Path.GetExtension(image.FileName);

                using(FileStream fs = new FileStream(Path.Combine(uploads, imageName + extension), FileMode.Create))
                {
                    image.CopyTo(fs);
                }

                _response.StatusCode = HttpStatusCode.Created;
                _response.Result = imageName + extension;

                return _response;
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
*/