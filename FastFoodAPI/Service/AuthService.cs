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


        public async Task<LoginResponseDTO> Login(LoginRequestDTO model)
        {
            var customer = await _userManager.Users.FirstOrDefaultAsync(u => u.Email == model.Email);

            var isValid = await _userManager.CheckPasswordAsync(customer, model.Password);
            if(isValid == false)
            {
                return new() { Customer = null, Token = "" };
            }

            // If user is found, generate a token
            var roles = await _userManager.GetRolesAsync(customer);
            var token = _jwtTokenGenerator.GenerateToken(customer, roles);

            CustomerDTO customerDTO = new CustomerDTO()
            {
                Id = customer.Id,
                Name = customer.Name,
                PhoneNumber = customer.PhoneNumber,
                Email = customer.Email,
                Address = customer.Address,
                City = customer.City,
            };

            LoginResponseDTO loginResponsetDTO = new LoginResponseDTO
            {

                Customer = customerDTO,
                Token = token,
            };

            return loginResponsetDTO;
        }

        public async Task<string> Register(RegisterRequestDTO model)
        {
            var user = _db.Users.FirstOrDefault(u =>  u.Email == model.Email );

            if (user != null)
            {
                return $"User Already exits";
            }

            Customer newUser = new Customer()
            {
                UserName = model.Email,
                Email = model.Email,
                NormalizedEmail = model.Email.ToUpper(),
                Name = model.Name,
                PhoneNumber = model.PhoneNumber,
                Address = model.Address,
                City = model.City,
            };

            var result = await _userManager.CreateAsync(newUser, model.Password);

            if (result.Succeeded)
            {
                if (!_roleManager.RoleExistsAsync(SD.Role_Admin).GetAwaiter().GetResult())
                {
                    // Creates role in database 
                    await _roleManager.CreateAsync(new IdentityRole(SD.Role_Admin));
                    if (model.Role.ToLower() == SD.Role_Admin.ToLower())
                    {

                        await _userManager.AddToRoleAsync(newUser, SD.Role_Admin);
                    }
       
                }
                else
                {
                    await _roleManager.CreateAsync(new IdentityRole(SD.Role_Customer));
                    if(model.Role.ToLower() == SD.Role_Customer.ToLower())
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
