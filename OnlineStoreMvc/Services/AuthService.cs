using NuGet.Protocol.Plugins;
using OnlineStore.Models.DTO;
using OnlineStore.Utility;
using OnlineStoreMvc.Models;
using OnlineStoreMvc.Services.IServices;

namespace OnlineStoreMvc.Services
{
    public class AuthService : BaseService, IAuthService
    {
        private readonly IHttpClientFactory _clientFactory;
        private string storeAPIUrl;
        public AuthService(IHttpClientFactory clientFactory, IConfiguration configuration) : base(clientFactory)
        {
            _clientFactory = clientFactory;
            storeAPIUrl = configuration.GetValue<string>("ServiceUrls:OnlineStoreAPI");
        }
        public Task<T> LoginAsync<T>(LoginRequestDTO loginRequest)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.POST,
                Data = loginRequest,
                Url = storeAPIUrl + "/api/UserAuth/login"
            });
        }

        public Task<T> RegisterAync<T>(RegistrationRequestDTO registrationRequest)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.POST,
                Data = registrationRequest,
                Url = storeAPIUrl + "/api/UserAuth/register"
            });
        }
    }
}
