using FastFoodAPI.Utility;
using FastFoodHouse_API.Models;
using FastFoodHouse_API.Models.Dtos;
using Moq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace FastFoodHouse_API.IntegrationTests.ControllerTests
{
    public class CustomControllerTests : IDisposable
    {
        private readonly CustomWebAplicationFactory _factory;
        private readonly HttpClient _client;

        public CustomControllerTests()
        {
            _factory = new CustomWebAplicationFactory();
            _client = _factory.CreateClient();
        }

        public void Dispose()
        {
            _factory.Dispose();
            _client.Dispose();
        }

        [Fact]

        public async Task GetCustomerByIdAsync_ReturnsOk_WhenCalledWithValidData()
        {
            
                // Arrange
                string id = "cd63c382-087c-49bc-b29e-e926bc3779ef";
                var customer = new Customer
                {
                    Id = id,
                    Name = "Test",
                    PhoneNumber = "1234567890",
                    Address = "Test",
                    City = "Test",
                };

                // Mocking the claim for the current user
                var claims = new List<Claim>
    {
        new Claim(ClaimTypes.Role, SD.Role_Admin),
        new Claim(ClaimTypes.NameIdentifier, id)
    };
                var identity = new ClaimsIdentity(claims);
                var principal = new ClaimsPrincipal(identity);

                // Set the current user on the HttpContext of the test client
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJjZDYzYzM4Mi0wODdjLTQ5YmMtYjI5ZS1lOTI2YmMzNzc5ZWYiLCJlbWFpbCI6IkFobWVkQGhvdG1haWwubm8iLCJuYW1lIjoiQWhtZWQgQWJkaSIsInJvbGUiOiJjdXN0b21lciIsIm5iZiI6MTcxNTE1MDgzMywiZXhwIjoxNzE1MjM3MjMzLCJpYXQiOjE3MTUxNTA4MzMsImlzcyI6ImZhc3Rmb29kaG91c2UtYXV0aC1hcGkiLCJhdWQiOiJmYXN0Zm9vZGhvdXNlLWNsaWVudCJ9.JWcQGXFcRPi0Sy438aIjiSgs02O8bqY0DhFgr7_sUgU");

            // Act
            _factory.CustomersRepoMock.Setup(x => x.GetCustomerById(id)).ReturnsAsync(customer);
            var response = await _client.GetAsync($"api/v1/customer/{id}");
                
                // Assert
                Assert.Equal(HttpStatusCode.OK, response.StatusCode);

                var data = JsonConvert.DeserializeObject<CustomerDTO>(await response.Content.ReadAsStringAsync());
                Assert.NotNull(data);
                Assert.Equal(id, data.Id);
            

        }


        [Fact]
        public async Task GetAllCustomer_WhenAsked_ReturnTwo_Customer()
        {
            // Arrange
            string id = "cd63c382-087c-49bc-b29e-e926bc3779ef";
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Role, SD.Role_Admin),
                new Claim(ClaimTypes.NameIdentifier, id)
            };
            var identity = new ClaimsIdentity(claims);
            var principal = new ClaimsPrincipal(identity);

            _factory.CustomersRepoMock.Setup(x => x.GetAllCustomers())
                .ReturnsAsync(new List<Customer>
                {
                    new Customer { Id = "customer-1-id", Name = "Maslah", Email = "zsdf@hotmail.com" },
                    new Customer { Id = "customer-2-id", Name = "Test", Email = "Test@hotmail.com" }
                });

            
            _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJhNGIzNWM1Ny1kYTcyLTQyZWItYWQ5OS05NGVjMzIwNmE1OTEiLCJlbWFpbCI6Ik1hc2xhaEBob3RtYWlsLm5vIiwibmFtZSI6Ik1hc2xhaCIsInJvbGUiOiJhZG1pbiIsIm5iZiI6MTcxNTE1NTMyMCwiZXhwIjoxNzE1MjQxNzIwLCJpYXQiOjE3MTUxNTUzMjAsImlzcyI6ImZhc3Rmb29kaG91c2UtYXV0aC1hcGkiLCJhdWQiOiJmYXN0Zm9vZGhvdXNlLWNsaWVudCJ9.KceiWL_0I06ChqS88eGDwDPQjVH8_cOXTO0901UHlvA");

            // Act
            var response = await _client.GetAsync("api/v1/customer");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var data = JsonConvert.DeserializeObject<IEnumerable<CustomerDTO>>(await response.Content.ReadAsStringAsync());
            Assert.NotNull(data);
            Assert.Collection(data, respons_customer =>
            {
                Assert.Equal("Maslah", respons_customer.Name);
                Assert.Equal("zsdf@hotmail.com", respons_customer.Email);
            }, r =>
            {
                Assert.Equal("Test", r.Name);
                Assert.Equal("Test@hotmail.com", r.Email);
            });
        }

    }
}
