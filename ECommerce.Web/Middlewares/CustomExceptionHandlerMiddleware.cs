using Shared.ErrorModels;
using System.Net;

namespace ECommerce.Web.Middlewares
{
    public class CustomExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<CustomExceptionHandlerMiddleware> _logger;
        public CustomExceptionHandlerMiddleware(RequestDelegate next, ILogger<CustomExceptionHandlerMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next.Invoke(context);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                //Response object with content type, status code
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                context.Response.ContentType = "application/json";
                var response = new ErrorDetails()
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError,
                    ErrorMessage = ex.Message
                };
                await context.Response.WriteAsJsonAsync(response);
            }
        }
    }
}
