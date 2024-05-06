using FastFoodHouse_API.Data;
using FastFoodHouse_API.Models;
using FastFoodHouse_API.Models.Dtos;
using FastFoodHouse_API.Service.Interface;
using FastFoodAPI.Utility;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;

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
                           IMapper mapper, IJwtTokenGenerator jwtTokenGenerator)
        {
            _db = applicationDbContext;
            _userManager = userManager;
            _roleManager = roleManager;
            _mapper = mapper;
            _jwtTokenGenerator = jwtTokenGenerator;
        }

        public async Task<LoginResponseDTO> Login(LoginRequestDTO model)
        {
            try
            {
                var customer = await _userManager.Users.FirstOrDefaultAsync(u => u.Email == model.Email);

                if (customer == null)
                {
                    return new LoginResponseDTO { Customer = null, Token = "" };
                }

                var isValid = await _userManager.CheckPasswordAsync(customer, model.Password);
                if (!isValid)
                {
                    return new LoginResponseDTO { Customer = null, Token = "" };
                }

                var roles = await _userManager.GetRolesAsync(customer);
                var token = _jwtTokenGenerator.GenerateToken(customer, roles);

                CustomerDTO customerDTO = _mapper.Map<CustomerDTO>(customer);

                return new LoginResponseDTO { Customer = customerDTO, Token = token };
            }
            catch (Exception ex)
            {
                // Log the exception if necessary
                Console.WriteLine($"An error occurred while logging in: {ex.Message}");
                throw;
            }
        }

        public async Task<string> Register(RegisterRequestDTO model)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(model.Email);

                if (user != null)
                {
                    return "User already exists";
                }

                var newUser = _mapper.Map<Customer>(model);
                newUser.UserName = model.Email;

                var result = await _userManager.CreateAsync(newUser, model.Password);

                if (result.Succeeded)
                {
                    var roleExists = await _roleManager.RoleExistsAsync(model.Role);
                    if (!roleExists)
                    {
                        await _roleManager.CreateAsync(new IdentityRole(model.Role));
                    }

                    await _userManager.AddToRoleAsync(newUser, model.Role);

                    return ""; // Success
                }
                else
                {
                    return result.Errors.FirstOrDefault()?.Description ?? "An error occurred during registration";
                }
            }
            catch (Exception ex)
            {
                // Log the exception if necessary
                Console.WriteLine($"An error occurred while registering: {ex.Message}");
                throw;
            }
        }
    }
}

