using OnlineStore.Models.DTO;
using OnlineStore.Utility;
using OnlineStoreMvc.Models;
using OnlineStoreMvc.Services.IServices;

namespace OnlineStoreMvc.Services
{
    public class ManufacturerService : BaseService, IManufacturerService
    {
        private readonly IHttpClientFactory _clientFactory;
        private string storeAPIUrl;

        public ManufacturerService(IHttpClientFactory clientFactory, IConfiguration configuration) : base(clientFactory)
        {
            _clientFactory = clientFactory;
            storeAPIUrl = configuration.GetValue<string>("ServiceUrls:OnlineStoreAPI");
        }

        public Task<T> CreateAsync<T>(ManufacturerCreateDTO dto, string token)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.POST,
                Data = dto,
                Url = storeAPIUrl + "/api/ManufacturerAPI",
                Token = token
            });
        }

        public Task<T> DeleteAsync<T>(int id, string token)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.DELETE,
                Url = storeAPIUrl + "/api/ManufacturerAPI/" + id,
                Token = token
            });
        }

        public Task<T> GetAllAsync<T>(string token)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.GET,
                Url = storeAPIUrl + "/api/ManufacturerAPI",
                Token = token
            });
        }

        public Task<T> GetAsync<T>(int id, string token)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.GET,
                Url = storeAPIUrl + "/api/ManufacturerAPI/" + id,
                Token = token
            });
        }

        public Task<T> UpdateAsync<T>(ManufacturerDTO dto, string token)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.PUT,
                Data = dto,
                Url = storeAPIUrl + "/api/ManufacturerAPI/" + dto.Id,
                Token = token
            });
        }
    }
}
