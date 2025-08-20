using DatabaseRepository.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace DatabaseRepository.Common.Middlewares
{
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionMiddleware> _logger;

        public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            int statusCode;
            string message;

            try
            {
                await _next(context); // Continue pipeline
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception occurred");

                context.Response.ContentType = "application/json";

                switch (ex)
                {
                    case UnauthorizedAccessException:
                        statusCode = StatusCodes.Status401Unauthorized;
                        message = "Unauthorized access.";
                        break;

                    case KeyNotFoundException:
                        statusCode = StatusCodes.Status404NotFound;
                        message = "Resource not found.";
                        break;

                    case ArgumentException:
                        statusCode = StatusCodes.Status400BadRequest;
                        message = ex.Message;
                        break;

                    default:
                        statusCode = StatusCodes.Status500InternalServerError;
                        message = "An unexpected error occurred.";
                        break;
                }

                var response = new ExceptionResponse
                {
                    StatusCode = context.Response.StatusCode = statusCode,
                    Message = message,
                    Detail = ex.Message // Optional: Remove in production for security
                };

                await context.Response.WriteAsync(JsonSerializer.Serialize(response));
            }
        }
    }

}
