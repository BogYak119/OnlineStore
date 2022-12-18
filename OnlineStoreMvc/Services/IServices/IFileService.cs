using OnlineStore.Models.DTO;

namespace OnlineStoreMvc.Services.IServices
{
    public interface IFileService
    {
        Task<T> PostAsync<T>(IFormFile file, string token);
    }
}
