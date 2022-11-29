using OnlineStore.Models;
using OnlineStore.Models.DTO;
using OnlineStore.Utility;
using OnlineStoreMvc.Models;
using OnlineStoreMvc.Services.IServices;

namespace OnlineStoreMvc.Services
{
    public class SubcategoryService : BaseService, ISubcategoryService
    {
        private readonly IHttpClientFactory _clientFactory;
        private string storeAPIUrl;

        public SubcategoryService(IHttpClientFactory clientFactory, IConfiguration configuration) : base(clientFactory)
        {
            _clientFactory = clientFactory;
            storeAPIUrl = configuration.GetValue<string>("ServiceUrls:OnlineStoreAPI");
        }

        public Task<T> CreateAsync<T>(SubcategoryCreateDTO dto, string token)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.POST,
                Data = dto,
                Url = storeAPIUrl + "/api/SubcategoryAPI",
                Token = token
            });
        }

        public Task<T> DeleteAsync<T>(int id, string token)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.DELETE,
                Url = storeAPIUrl + "/api/SubcategoryAPI/" + id,
                Token = token
            });
        }

        public Task<T> GetAllAsync<T>(string token)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.GET,
                Url = storeAPIUrl + "/api/SubcategoryAPI",
                Token = token
            });
        }

        public Task<T> GetAsync<T>(int id, string token)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.GET,
                Url = storeAPIUrl + "/api/SubcategoryAPI/" + id,
                Token = token
            });
        }

        public Task<T> UpdateAsync<T>(SubcategoryDTO dto, string token)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.PUT,
                Data = dto,
                Url = storeAPIUrl + "/api/SubcategoryAPI/" + dto.Id,
                Token = token
            });
        }
    }
}
