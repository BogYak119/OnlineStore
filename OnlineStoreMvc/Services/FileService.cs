using OnlineStore.Models.DTO;
using OnlineStore.Utility;
using OnlineStoreMvc.Models;
using OnlineStoreMvc.Services.IServices;

namespace OnlineStoreMvc.Services
{
    public class FileService : BaseService, IFileService
    {
        private readonly IHttpClientFactory _clientFactory;
        private string storeAPIUrl;

        public FileService(IHttpClientFactory clientFactory, IConfiguration configuration) : base(clientFactory)
        {
            _clientFactory = clientFactory;
            storeAPIUrl = configuration.GetValue<string>("ServiceUrls:OnlineStoreAPI");
        }

        public Task<T> PostAsync<T>(IFormFile file, string token)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.POST,
                Data = file,
                Url = storeAPIUrl + "/api/FileAPIController",
                Token = token
            });
        }
    }
}
