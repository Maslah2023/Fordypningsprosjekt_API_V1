namespace FastFoodHouse_API.Middleware
{
    using Microsoft.AspNetCore.Mvc;
    using System.Net;
    using System.Text.Json;

    namespace GokstadHageVennerAPI.Middleware
    {
        public class GlobalExceptionMiddleware : IMiddleware
        {
            private readonly ILogger<GlobalExceptionMiddleware> _logger;

                public GlobalExceptionMiddleware(ILogger<GlobalExceptionMiddleware> logger)
            {
                _logger = logger;
            }

            public async Task InvokeAsync(HttpContext context, RequestDelegate next)
            {
                try
                {
                    await next(context);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, ex.Message);
                    context.Response.StatusCode =
                        (int)HttpStatusCode.InternalServerError;
                    ProblemDetails problem = new()
                    {
                        Status = (int)HttpStatusCode.InternalServerError,
                        Type = "Server error",
                        Title = "Server error",
                        Detail = ex.Message.ToString()
                    };


                    string json = JsonSerializer.Serialize(problem);
                    context.Response.ContentType = "application/json";
                    await context.Response.WriteAsync(json);
                }
            }
        }
    }
}
