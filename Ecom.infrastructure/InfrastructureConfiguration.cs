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
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.Cookies;
using Ecom.core.Entites;
using Microsoft.AspNetCore.Identity;

namespace Ecom.infrastructure
{
    public static class InfrastructureConfiguration
    {
        public static IServiceCollection infrastructureConfiguration( this IServiceCollection services , IConfiguration  configuration)
        {

            
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

            // Register token 
            services.AddScoped<IGenerateToken, GenerateToken>();


            //Register Email sender 
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IOrderService, OrderService>();

            services.AddSingleton<IImageManagemntServce, ImageManagemntServce>();
            services.AddSingleton<IFileProvider>(new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot")));

        
            // Apply Dbcontext
            services.AddDbContext<AppDbContext>(op =>
            {
                op.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));

            });

            services.AddIdentity<AppUser, IdentityRole>().AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();
            //Authentication
            services.AddAuthentication(op =>
            {
                op.DefaultAuthenticateScheme  =JwtBearerDefaults.AuthenticationScheme;
                op.DefaultChallengeScheme =JwtBearerDefaults.AuthenticationScheme;  
                op.DefaultScheme =CookieAuthenticationDefaults.AuthenticationScheme;
            }).AddCookie(o =>
            {
                o.Cookie.Name = "token";
                o.Events.OnRedirectToLogin = context =>
                {
                    context.Response.StatusCode = 401; // Unauthorized
                    return Task.CompletedTask;
                };
            }).AddJwtBearer(op =>
            {
                op.RequireHttpsMetadata = false;
                op.SaveToken = true;
                op.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Token:Secret"])),
                    ValidateIssuer = true,
                    ValidateAudience = false,
                    ValidIssuer = configuration["Token:Issure"],
                    ClockSkew = TimeSpan.Zero // Disable clock skew
                }; 
                op.Events= new JwtBearerEvents()
                {
                    OnMessageReceived = context =>
                    {
                      context.Token = context.Request.Cookies["token"];
                        return Task.CompletedTask;
                    }
                };
            });


            return services;
        }
    }
}
