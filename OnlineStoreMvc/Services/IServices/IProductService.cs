using OnlineStore.Models.DTO;

namespace OnlineStoreMvc.Services.IServices
{
    public interface IProductService
    {
        Task<T> GetAllAsync<T>();
        Task<T> GetAsync<T>(int id);
        Task<T> CreateAsync<T>(ProductCreateDTO dto);
        Task<T> UpdateAsync<T>(ProductDTO dto);
        Task<T> DeleteAsync<T>(int id);
    }
}
