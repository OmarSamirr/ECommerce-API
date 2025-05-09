using Domain.Exceptions;
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
                //If Endpoint is not found
                await HandleNotFoundEndpoint(context);
            }
            catch (Exception ex)
            {
                await HandleCatchException(context, ex);
            }
        }

        private async Task HandleCatchException(HttpContext context, Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            //Response object with content type, status code

            context.Response.ContentType = "application/json";
            var response = new ErrorDetails()
            {
                ErrorMessage = ex.Message
            };

            response.StatusCode = ex switch
            {
                NotFoundException => (int)HttpStatusCode.NotFound,
                _ => (int)HttpStatusCode.InternalServerError
            };
            context.Response.StatusCode = response.StatusCode;

            //Return as Json
            await context.Response.WriteAsJsonAsync(response);
        }

        private static async Task HandleNotFoundEndpoint(HttpContext context)
        {
            if (context.Response.StatusCode == (int)HttpStatusCode.NotFound)
            {
                context.Response.ContentType = "application/json";
                var response = new ErrorDetails()
                {
                    StatusCode = (int)HttpStatusCode.NotFound,
                    ErrorMessage = $"Endpoint with this path: {context.Request.Path} is not found."
                };
                //Return as Json
                await context.Response.WriteAsJsonAsync(response);
            }
        }
    }
}
