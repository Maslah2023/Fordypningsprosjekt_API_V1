using FastFoodHouse_API.Data;
using FastFoodHouse_API.Models;
using FastFoodHouse_API.Service.Interface;
using FastFoodHouse_API.Service;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Writers;
using System;
using System.Data.Common;
using FastFoodHouse_API.Repository.Interface;
using FastFoodHouse_API.Repository;
using Serilog;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;
using FastFoodHouse_API.Extensions;
using FastFoodHouse_API.Middleware.GokstadHageVennerAPI.Middleware;
using FastFoodHouse_API.Middleware;
using FastFoodHouse_API.Validators;
using FluentValidation.AspNetCore;
using System.Reflection;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(opt =>
{
    opt.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
  
});
//builder.Services.AddDbContext<ApplicationDbContext>(options =>
//    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")),
//    ServiceLifetime.Singleton);

builder.Services.AddIdentity<Customer, IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

IServiceCollection serviceCollection = builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection("Appsetnings:JwtOptions"));
builder.Services.AddScoped<ICartItemService, CartItemService>();
builder.Services.AddScoped<ICartItemRepo, CartItemRepo>();
builder.Services.AddScoped<IShoppingCartService, ShoppingCartService>(); 
builder.Services.AddScoped<IShoppingCartRepo, ShoppingCartRepo>();
builder.Services.AddScoped<IMenuService, MenuItemService>();
builder.Services.AddScoped<IMenuRepo, MenuRepo>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IOrderRepo, OrderRepo>();
builder.Services.AddScoped<ICustomerRepo, CustomerRepo>();
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
builder.Services.AddTransient<GlobalExceptionMiddleware>();




builder.Services.AddControllers()
    .AddFluentValidation(c => 
    c.RegisterValidatorsFromAssembly(Assembly.GetExecutingAssembly()));
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();



builder.Services.AddSwaggerGen(option =>
{
    option.AddSecurityDefinition(name: JwtBearerDefaults.AuthenticationScheme, securityScheme: new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Description = "Enter the Bearer Authorization string as following: `Bearer Generated-JWT-Token`",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
    option.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference= new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id=JwtBearerDefaults.AuthenticationScheme
                }
            }, new string[]{}
        }
    });
});
builder.AddAppAuthetication();



builder.Host.UseSerilog((context, Configuration) =>
{
    Configuration.ReadFrom.Configuration(context.Configuration);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseSerilogRequestLogging();
app.UseHttpsRedirection();
app.UseMiddleware<GlobalExceptionMiddleware>();

app.UseAuthorization();

app.MapControllers();
ApplyMigration();
app.Run();


void ApplyMigration()
{
    using (var scope = app.Services.CreateScope())
    {
        var _db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        if (_db.Database.GetPendingMigrations().Count() > 0)
        {
            _db.Database.Migrate();
        }
    }
}