using OnlineStore.Models.DTO;

namespace OnlineStoreMvc.Services.IServices
{
    public interface ISubcategoryService
    {
        Task<T> GetAllAsync<T>();
        Task<T> GetAsync<T>(int id);
        Task<T> CreateAsync<T>(SubcategoryCreateDTO dto);
        Task<T> UpdateAsync<T>(SubcategoryDTO dto);
        Task<T> DeleteAsync<T>(int id);
    }
}
