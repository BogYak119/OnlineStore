using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters.Internal;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using OnlineStore.DataAccess.Data;
using OnlineStore.DataAccess.Repository.IRepository;
using OnlineStore.Models;
using OnlineStore.Models.DTO;
using OnlineStore.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace OnlineStore.DataAccess.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMapper _mapper;
        private readonly IPasswordHasher<ApplicationUser> _passwordHasher;
        private string secretKey;
        public UserRepository(ApplicationDbContext db, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager,
            IMapper mapper, IConfiguration configuration, IPasswordHasher<ApplicationUser> passwordHasher)
        {
            _db = db;
            _userManager = userManager;
            _roleManager = roleManager;
            _mapper = mapper;
            _passwordHasher = passwordHasher;
            secretKey = configuration.GetValue<string>("ApiSettings:Secret");
        }
        public bool isUniqueUser(string username)
        {
            var user = _db.ApplicationUsers.FirstOrDefault(u => u.UserName == username);
            return user == null ? true : false;
        }


        public async Task<LoginResponseDTO> Login(LoginRequestDTO loginRequestDTO)
        {
            ApplicationUser? user = _db.ApplicationUsers.FirstOrDefault(u => u.UserName == loginRequestDTO.UserName);

            bool passwordIsValid = await _userManager.CheckPasswordAsync(user, loginRequestDTO.Password);

            if (user == null || passwordIsValid == false)
            {
                return new LoginResponseDTO()
                {
                    Token = "",
                    User = null
                };
            }

            var roles = await _userManager.GetRolesAsync(user);
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(secretKey);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.UserName.ToString()),
                    new Claim(ClaimTypes.Role, roles.FirstOrDefault())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            SecurityToken? token = tokenHandler.CreateToken(tokenDescriptor);

            LoginResponseDTO loginResponseDTO = new LoginResponseDTO()
            {
                Token = tokenHandler.WriteToken(token),
                User = _mapper.Map<UserDTO>(user),
            };

            return loginResponseDTO;
        }


        public async Task<RegistrationResponseDTO> Register(RegistrationRequestDTO registrationRequestDTO)
        {
            ApplicationUser user = new ApplicationUser()
            {
                UserName = registrationRequestDTO.UserName,
                Email = registrationRequestDTO.UserName,
                NormalizedEmail = registrationRequestDTO.UserName.ToLower(),
                Name = registrationRequestDTO.Name,
            };
            try
            {
                RegistrationResponseDTO? registarionResponse;
                var result = await _userManager.CreateAsync(user, registrationRequestDTO.Password);
                if (result.Succeeded)
                {
                    if (!_roleManager.RoleExistsAsync("admin").GetAwaiter().GetResult())
                    {
                        await _roleManager.CreateAsync(new IdentityRole("admin"));
                        await _roleManager.CreateAsync(new IdentityRole("customer"));
                    }
                    if (registrationRequestDTO.Role == null
                        || !_roleManager.RoleExistsAsync(registrationRequestDTO.Role).GetAwaiter().GetResult())
                    {
                        await _userManager.AddToRoleAsync(user, "customer");
                    }
                    else
                    {
                        await _userManager.AddToRoleAsync(user, registrationRequestDTO.Role);
                    }
                    var userToReturn = _db.ApplicationUsers.FirstOrDefault(u => u.UserName == registrationRequestDTO.UserName);
                    registarionResponse = new RegistrationResponseDTO()
                    {
                        userDTO = _mapper.Map<UserDTO>(userToReturn),
                        Errors = null
                    };
                }
                else
                {
                    registarionResponse = new RegistrationResponseDTO()
                    {
                        userDTO = null,
                        Errors = new List<string>()
                    };
                    foreach (var error in result.Errors)
                    {
                        registarionResponse.Errors.Add(error.Description);
                    }
                }
                return registarionResponse;
            }

            catch (Exception ex)
            {

            }

            return null;
        }


        public async Task<List<UserDTO>> GetAllAsync(Expression<Func<ApplicationUser, bool>>? filter = null)
        {
            List<UserDTO> result = new List<UserDTO>();
            IQueryable<ApplicationUser> query = _db.Users.Where(u => u.UserName != "admin@gmail.com");

            if (filter != null)
            {
                query = query.Where(filter);
            }

            foreach (var user in query.ToList())
            {
                UserDTO userDTO = _mapper.Map<UserDTO>(user);
                userDTO.Role = _userManager.GetRolesAsync(user).Result.FirstOrDefault();
                result.Add(userDTO);
            }

            return result;
        }


        public async Task<UserDTO> GetAsync(Expression<Func<ApplicationUser, bool>>? filter = null, bool tracked = true)
        {
            IQueryable<ApplicationUser> query = _db.Users;

            if (!tracked)
                query = query.AsNoTracking();


            if (filter != null)
                query = query.Where(filter);

            var user = query.FirstOrDefault();

            if (user != null)
            {
                UserDTO userDTO = _mapper.Map<UserDTO>(user);
                userDTO.Role = _userManager.GetRolesAsync(user).Result.FirstOrDefault();
                return userDTO;
            }

            return null;
        }


        public async Task<List<string>> RemoveAsync(string id)
        {
            List<string> errorList = new List<string>();
            var user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                var result = await _userManager.DeleteAsync(user);
                if (!result.Succeeded)
                {
                    foreach (var error in result.Errors)
                    {
                        errorList.Add(error.Description);
                    }
                }
            }
            return errorList;
        }



        public async Task<ApplicationUser> UpdateAsync(UserDTO userDTO)
        {
            var user = await _userManager.FindByIdAsync(userDTO.ID);

            if (user != null)
            {
                user.Name = userDTO.Name;
                user.UserName = userDTO.UserName;
                if (userDTO.Password != null)
                {
                    user.PasswordHash = _passwordHasher.HashPassword(user, userDTO.Password);
                }
                await _userManager.UpdateAsync(user);
                if (userDTO.Role != null)
                {
                    var oldRole = (await GetAsync(u => u.Id == userDTO.ID)).Role;
                    if (oldRole != userDTO.Role && _roleManager.RoleExistsAsync(userDTO.Role).GetAwaiter().GetResult())
                    {
                        await _userManager.RemoveFromRoleAsync(user, oldRole);
                        await _userManager.AddToRoleAsync(user, userDTO.Role);
                    }
                }
                return user;
            }
            return null;
        }
    }
}
