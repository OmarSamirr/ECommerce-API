using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Services.MappingProfiles;
using ServicesAbstraction;
using Shared.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public static class ApplicationServiceRegistration
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services,
                                                                     IConfiguration configuration)
        {
            //Old Service Manager Using Lazy
            //services.AddScoped<IServiceManager, ServiceManager>();

            //New Service Manager Using Delegate.Invoke()
            services.AddScoped<IServiceManager, ServiceManagerWithFactoryDelegate>();

            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddScoped<IBasketService, BasketService>();
            services.AddScoped<IOrderService, OrderService>();
            
            services.AddScoped<Func<IProductService>>(provider => ()
            => provider.GetRequiredService<IProductService>());

            services.AddScoped<Func<IBasketService>>(provider => ()
            => provider.GetRequiredService<IBasketService>());
            
            services.AddScoped<Func<IAuthenticationService>>(provider => ()
            => provider.GetRequiredService<IAuthenticationService>());
            
            services.AddScoped<Func<IOrderService>>(provider => ()
            => provider.GetRequiredService<IOrderService>());

            services.AddAutoMapper(typeof(ProductProfile).Assembly);
            services.Configure<JwtOptions>(configuration.GetSection("JWTOptions"));
            return services;
        }

    }
}
