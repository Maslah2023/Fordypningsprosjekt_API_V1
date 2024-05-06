using FastFoodHouse_API.Models.Dtos;
using FastFoodHouse_API.Validators;
using FluentValidation.Results;
using Newtonsoft.Json;

namespace FastFoodHouse_API.Middleware
{
    public class ValidationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly RegisterRequestDtoValidators _validator;

        public ValidationMiddleware(RequestDelegate next, RegisterRequestDtoValidators validator)
        {
            _next = next;
            _validator = validator;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (context.Request.Path.StartsWithSegments("/register")) // Only apply validation to specific endpoint(s)
            {
                // Read request body
                string requestBody = await new System.IO.StreamReader(context.Request.Body).ReadToEndAsync();
                var requestDto = JsonConvert.DeserializeObject<RegisterRequestDTO>(requestBody);

                // Validate request DTO
                ValidationResult validationResult = _validator.Validate(requestDto);

                // Check if validation failed
                if (!validationResult.IsValid)
                {
                    // Return validation errors
                    context.Response.StatusCode = StatusCodes.Status400BadRequest;
                    context.Response.ContentType = "application/json";
                    await context.Response.WriteAsync(JsonConvert.SerializeObject(validationResult.Errors));
                    return;
                }
            }

            // If validation succeeded or the request is not for registration, continue to the next middleware
            await _next(context);
        }
    }
}
