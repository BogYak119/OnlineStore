using OnlineStore.Models.DTO;

namespace OnlineStoreMvc.Services.IServices
{
    public interface IManufacturerService
    {
        Task<T> GetAllAsync<T>(string token);
        Task<T> GetAsync<T>(int id, string token);
        Task<T> CreateAsync<T>(ManufacturerCreateDTO dto, string token);
        Task<T> UpdateAsync<T>(ManufacturerDTO dto, string token);
        Task<T> DeleteAsync<T>(int id, string token);
    }
}
