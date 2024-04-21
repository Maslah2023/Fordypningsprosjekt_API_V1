using AutoMapper;
using FastFoodHouse_API.Data;
using FastFoodHouse_API.Models;
using FastFoodHouse_API.Models.Dtos;
using FastFoodHouse_API.Service.Interface;
using FastFoodAPI.Utility;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace FastFoodHouse_API.Service
{
    public class AuthService : IAuthService
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<Customer> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMapper _mapper;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;
    

        public AuthService(ApplicationDbContext applicationDbContext, 
                           UserManager<Customer> userManager, 
                           RoleManager<IdentityRole> roleManager, 
                           IMapper mapper, IJwtTokenGenerator jwtTokenGenerator
                         )
        {
            _db = applicationDbContext;
            _userManager = userManager;
            _roleManager = roleManager;
            _mapper = mapper;
            _jwtTokenGenerator = jwtTokenGenerator;
  
        }

        public async Task<string> DeleteUser(string userId)
        {
            var userToDelete = await  _userManager.FindByIdAsync(userId);
            if (userToDelete == null)
            {
                return "Not Found";
            }
            else
            {
                await _userManager.DeleteAsync(userToDelete);
            }
            return "";
        }
        public async Task<IEnumerable<UserDto>> GetAllUsers()
        {
            var users = await _db.Customers.ToListAsync();
            if (users == null)
            {
                throw new ArgumentNullException(nameof(users));
            }
            IEnumerable<UserDto> userDto = _mapper.Map<IEnumerable<UserDto>>(users);
            return userDto;
        }

        public async Task<UserDto> GetCustomerById(string id)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.Id == id);
            UserDto userDto = _mapper.Map<UserDto>(user);
            return userDto;

        }

        public async Task<LoginResponseDTO> Login(LoginRequestDTO model)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.UserName == model.UserName);

            var isValid = await _userManager.CheckPasswordAsync(user, model.Password);
            if(isValid == false)
            {
                return new() { User = null, Token = "" };
            }

            // If user is found, generate a token
            var roles = await _userManager.GetRolesAsync(user);
            var token = _jwtTokenGenerator.GenerateToken(user, roles);

            UserDto userDto = new UserDto()
            {
                Id = user.Id,
                Name = user.Name,
                PhoneNumber = user.PhoneNumber,
                Email = user.Email
            };

            LoginResponseDTO loginResponsetDTO = new LoginResponseDTO
            {

                User = userDto,
                Token = token,
            };

            return loginResponsetDTO;
        }

        public async Task<string> Register(RegisterRequestDTO model)
        {
            var user = _db.Users.FirstOrDefault(u => u.UserName.ToLower()  == model.UserName.ToLower() || u.Email == model.UserName );

            if (user != null)
            {
                return $"User Already exits";
            }

            Customer newUser = new Customer()
            {
                UserName = model.UserName,
                NormalizedEmail = model.UserName.ToUpper(),
                Name = model.Name,

            };

            var result = await _userManager.CreateAsync(newUser, model.Password);

            if (result.Succeeded)
            {
                if (!_roleManager.RoleExistsAsync(SD.Role_Admin).GetAwaiter().GetResult())
                {
                    // Creates role in database 
                    await _roleManager.CreateAsync(new IdentityRole(SD.Role_Admin));
                    await _roleManager.CreateAsync(new IdentityRole(SD.Role_Customer));
                    if (model.Role.ToLower() == SD.Role_Admin.ToLower())
                    {

                        await _userManager.AddToRoleAsync(newUser, SD.Role_Admin);
                    }
                    else
                    {
                        await _userManager.AddToRoleAsync(newUser, SD.Role_Customer);
                    }
                }
            }
            else
            {
                return result.Errors.FirstOrDefault().Description;
            }
            return "";



        }
    }
}
