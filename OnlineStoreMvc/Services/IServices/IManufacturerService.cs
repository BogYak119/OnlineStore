using OnlineStore.Models.DTO;

namespace OnlineStoreMvc.Services.IServices
{
    public interface IManufacturerService
    {
        Task<T> GetAllAsync<T>();
        Task<T> GetAsync<T>(int id);
        Task<T> CreateAsync<T>(ManufacturerCreateDTO dto);
        Task<T> UpdateAsync<T>(ManufacturerDTO dto);
        Task<T> DeleteAsync<T>(int id);
    }
}
