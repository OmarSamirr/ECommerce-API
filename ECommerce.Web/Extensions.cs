using Domain.Contracts;
using ECommerce.Web.Factories;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Web
{
    public static class Extensions
    {
        public static IServiceCollection AddWebApplicationService(this IServiceCollection services)
        {
            services.AddControllers();
            services.AddSwaggerServices();
            services.Configure<ApiBehaviorOptions>(options =>
            {
                //Func<ActionContext,IActionResult>
                options.InvalidModelStateResponseFactory = ApiResponseFactory.GenerateApiValidationResponse;
            });
            return services;
        }

        private static void AddSwaggerServices(this IServiceCollection services)
        {
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
        }

        public static async Task InitializeDbAsync(this WebApplication app)
        {
            //Create Scope For Seeding
            using var scope = app.Services.CreateScope();
            var dbInitializer = scope.ServiceProvider.GetRequiredService<IDbInitializer>();
            await dbInitializer.InitializeAsync();
        }
    }
}
