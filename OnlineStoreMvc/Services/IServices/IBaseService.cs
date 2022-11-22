using OnlineStore.Models;
using OnlineStoreMvc.Models;

namespace OnlineStoreMvc.Services.IServices
{
    public interface IBaseService
    {
        APIResponse responseModel { get; set; }
        Task<T> SendAsync<T>(APIRequest request);
    }
}
