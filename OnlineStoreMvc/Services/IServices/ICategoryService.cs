using OnlineStore.Models.DTO;

namespace OnlineStoreMvc.Services.IServices
{
    public interface ICategoryService
    {
        Task<T> GetAllAsync<T>(string token);
        Task<T> GetAsync<T>(int id, string token);
        Task<T> CreateAsync<T>(CategoryCreateDTO dto, string token);
        Task<T> UpdateAsync<T>(CategoryDTO dto, string token);
        Task<T> DeleteAsync<T>(int id, string token);
    }
}
