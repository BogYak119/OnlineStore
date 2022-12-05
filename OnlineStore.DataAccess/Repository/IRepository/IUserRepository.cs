using OnlineStore.Models;
using OnlineStore.Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace OnlineStore.DataAccess.Repository.IRepository
{
    public interface IUserRepository
    {
        bool isUniqueUser(string username);
        Task<LoginResponseDTO> Login(LoginRequestDTO loginRequestDTO);
        Task<RegistrationResponseDTO> Register(RegistrationRequestDTO registrationRequestDTO);
        Task<List<UserDTO>> GetAllAsync(Expression<Func<ApplicationUser, bool>>? filter = null);
        Task<UserDTO> GetAsync(Expression<Func<ApplicationUser, bool>>? filter = null, bool tracked = true);
        Task<List<string>> RemoveAsync(string id);
        Task<ApplicationUser> UpdateAsync(UserDTO userDTO);

    }
}
