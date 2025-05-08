
using Domain.Contracts;
using ECommerce.Web.Middlewares;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Persistence;
using Persistence.Data;
using Persistence.Repositories;
using Services;
using Services.MappingProfiles;
using ServicesAbstraction;
using Shared.ErrorModels;

namespace ECommerce.Web
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddDbContext<StoreDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });

            builder.Services.AddScoped<IDbInitializer, DbInitializer>();
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped<IServiceManager, ServiceManager>();

            builder.Services.AddAutoMapper(typeof(ProductProfile).Assembly);
            builder.Services.Configure<ApiBehaviorOptions>(options =>
            {
                //Func<ActionContext,IActionResult>
                options.InvalidModelStateResponseFactory = (context) =>
                {
                    var errors = context.ModelState.Where(modelStateEntry => modelStateEntry.Value.Errors.Any())
                    .Select(modelStateEntry => new ValidationError()
                    {
                        Field = modelStateEntry.Key,
                        Errors = modelStateEntry.Value.Errors.Select(err => err.ErrorMessage)
                    });
                    var response = new ValidationErrorModel()
                    {
                        ValidationErrors = errors
                    };
                    return new BadRequestObjectResult(response);
                };
            });

            var app = builder.Build();

            await InitializeDbAsync(app);

            app.UseMiddleware<CustomExceptionHandlerMiddleware>();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseStaticFiles();
            app.UseHttpsRedirection();

            //app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
        public static async Task InitializeDbAsync(WebApplication app)
        {
            //Create Scope For Seeding
            using var scope = app.Services.CreateScope();
            var dbInitializer = scope.ServiceProvider.GetRequiredService<IDbInitializer>();
            await dbInitializer.InitializeAsync();
        }
    }
}
