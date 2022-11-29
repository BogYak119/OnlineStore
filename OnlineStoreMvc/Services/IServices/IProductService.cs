using OnlineStore.Models.DTO;

namespace OnlineStoreMvc.Services.IServices
{
    public interface IProductService
    {
        Task<T> GetAllAsync<T>(string token);
        Task<T> GetAsync<T>(int id, string token);
        Task<T> CreateAsync<T>(ProductCreateDTO dto, string token);
        Task<T> UpdateAsync<T>(ProductDTO dto, string token);
        Task<T> DeleteAsync<T>(int id, string token);
    }
}
