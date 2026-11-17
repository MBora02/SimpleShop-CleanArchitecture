using Microsoft.Extensions.DependencyInjection;
using SimpleShop.Application.Interfaces.Services;
using SimpleShop.Application.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleShop.Application
{
    public static class ServiceRegistration
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IProductService, ProductService>();
            return services;
        }
    }
}
