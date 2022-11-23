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

        public Task<T> CreateAsync<T>(SubcategoryCreateDTO dto)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.POST,
                Data = dto,
                Url = storeAPIUrl + "/api/SubcategoryAPI"
            });
        }

        public Task<T> DeleteAsync<T>(int id)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.DELETE,
                Url = storeAPIUrl + "/api/SubcategoryAPI/" + id
            });
        }

        public Task<T> GetAllAsync<T>()
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.GET,
                Url = storeAPIUrl + "/api/SubcategoryAPI"
            });
        }

        public Task<T> GetAsync<T>(int id)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.GET,
                Url = storeAPIUrl + "/api/SubcategoryAPI/" + id
            });
        }

        public Task<T> UpdateAsync<T>(SubcategoryDTO dto)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.PUT,
                Data = dto,
                Url = storeAPIUrl + "/api/SubcategoryAPI/" + dto.Id
            });
        }
    }
}
