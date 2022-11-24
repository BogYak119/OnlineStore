using Microsoft.Extensions.Configuration;
using OnlineStore.DataAccess.Data;
using OnlineStore.DataAccess.Repository.IRepository;
using OnlineStore.Models;
using OnlineStore.Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineStore.DataAccess.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _db;
        private string secretKey;
        public UserRepository(ApplicationDbContext db, IConfiguration configuration)
        {
            _db = db;
        }
        public bool isUniqueUser(string username)
        {
            LocalUser user = _db.LocalUsers.FirstOrDefault(u => u.UserName == username);
            return user == null ? false : true;
        }

        public Task<LoginResponseDTO> Login(LoginRequestDTO loginRequestDTO)
        {
            LocalUser user = _db.LocalUsers.FirstOrDefault(u => u.UserName == loginRequestDTO.UserName
            && u.Password == loginRequestDTO.Password);

            if (user == null)
                return null;

            return null;
        }

        public async Task<LocalUser> Register(RegistrationRequestDTO registrationRequestDTO)
        {
            LocalUser user = new LocalUser()
            {
                UserName = registrationRequestDTO.UserName,
                Password = registrationRequestDTO.Password,
                Name = registrationRequestDTO.Name,
                Role = registrationRequestDTO.Role,
            };
            await _db.LocalUsers.AddAsync(user);
            await _db.SaveChangesAsync();

            user.Password = "";
            return user;
        }
    }
}
