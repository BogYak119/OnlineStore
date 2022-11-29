using OnlineStore.Models.DTO;

namespace OnlineStoreMvc.Services.IServices
{
    public interface ISubcategoryService
    {
        Task<T> GetAllAsync<T>(string token);
        Task<T> GetAsync<T>(int id, string token);
        Task<T> CreateAsync<T>(SubcategoryCreateDTO dto, string token);
        Task<T> UpdateAsync<T>(SubcategoryDTO dto, string token);
        Task<T> DeleteAsync<T>(int id, string token);
    }
}
