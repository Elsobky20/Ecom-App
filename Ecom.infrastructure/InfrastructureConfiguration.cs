using Ecom.core.Interfaces;
using Ecom.infrastructure.Data;
using Ecom.infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;
using Microsoft.Extensions;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Ecom.core.Services;
using Ecom.infrastructure.Repositories.Service;
using Microsoft.Extensions.FileProviders;
using StackExchange.Redis;

namespace Ecom.infrastructure
{
    public static class InfrastructureConfiguration
    {
        public static IServiceCollection infrastructureConfiguration( this IServiceCollection services , IConfiguration  configuration)
        {

            // Apply Dbcontext
            services.AddDbContext<AppDbContext>(op =>
            {
                op.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));

            });
            //apply Redis connection
            services.AddSingleton<IConnectionMultiplexer>(i =>
            {
                var config = ConfigurationOptions.Parse(configuration.GetConnectionString("redis"));
                config.AbortOnConnectFail = false;
                config.AsyncTimeout = 10000;
                config.SyncTimeout = 10000;
                return ConnectionMultiplexer.Connect(config);
            });

            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            //Apply UnitOfWork
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddSingleton<IImageManagemntServce, ImageManagemntServce>();
            services.AddSingleton<IFileProvider>(new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot")));

            
            return services;
        }
    }
}
