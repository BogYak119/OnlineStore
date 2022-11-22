using OnlineStore.Models.DTO;

namespace OnlineStoreMvc.Services.IServices
{
    public interface ICategoryService
    {
        Task<T> GetAllAsync<T>();
        Task<T> GetAsync<T>(int id);
        Task<T> CreateAsync<T>(CategoryCreateDTO dto);
        Task<T> UpdateAsync<T>(CategoryDTO dto);
        Task<T> DeleteAsync<T>(int id);
    }
}
