using FastFoodHouse_API.Repository.Interface;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastFoodHouse_API.IntegrationTests
{
    internal class CustomWebAplicationFactory : WebApplicationFactory<Program>
    {
        public CustomWebAplicationFactory()
        {
            CustomersRepoMock = new Mock<ICustomerRepo>();

        }
        public Mock<ICustomerRepo> CustomersRepoMock { get; }


        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            base.ConfigureWebHost(builder);

            builder.ConfigureTestServices(services =>
            {
                services.AddSingleton(CustomersRepoMock.Object);
            });
        }

    }
}
