using OnlineStore.Models.DTO;

namespace OnlineStoreMvc.Services.IServices
{
    public interface IUserService
    {
        Task<T> GetAllAsync<T>(string token);
        Task<T> GetAsync<T>(string id, string token);
        Task<T> DeleteAsync<T>(string id, string token);
        Task<T> UpdateAsync<T>(UserDTO userDTO, string token);
    }
}
