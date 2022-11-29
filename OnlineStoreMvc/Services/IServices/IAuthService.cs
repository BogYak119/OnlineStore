using OnlineStore.DataAccess.Migrations;
using OnlineStore.Models;
using OnlineStore.Models.DTO;

namespace OnlineStoreMvc.Services.IServices
{
    public interface IAuthService
    {
        Task<T> LoginAsync<T>(LoginRequestDTO loginRequest);
        Task<T> RegisterAync<T>(RegistrationRequestDTO registrationRequest);
    }
}
