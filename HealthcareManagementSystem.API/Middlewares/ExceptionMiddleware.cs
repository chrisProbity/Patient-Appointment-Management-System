using HealthcareManagementSystem.Data.DTOs.Response;
using System.Net;
using System.Text.Json;

namespace HealthcareManagementSystem.API.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _log;
        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> log)
        {
            _log = log;
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(httpContext, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var message = $"An error occured please try again later";

            string stackTrace = string.Format("Exception Occured!!!  => Context => {0} => {1} | {2} | {3}", context?.Request?.Path.Value, exception?.Message, exception?.InnerException?.InnerException, exception?.StackTrace);

            var result = new GlobalResponse<string>
            {
                Status = false,
                StatusCode = 500,
                Message = message,
            };

            _log.LogInformation(stackTrace);

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            return context.Response.WriteAsync(JsonSerializer.Serialize(result));
        }

    }
}
