using Ecom.API.Helper;
using Microsoft.Extensions.Caching.Memory;
using System.Net;
using System.Text.Json;

namespace Ecom.API.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IHostEnvironment _environment;
        private readonly IMemoryCache _memoryCache; //must to rejister in program.cs
        private readonly TimeSpan _rateLimitWindow = TimeSpan.FromSeconds(30);

        public ExceptionMiddleware(RequestDelegate next, IHostEnvironment environment , IMemoryCache memoryCache)
        {
            _next = next;
            _environment = environment;
            _memoryCache = memoryCache;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                ApplaySecurityHeaders(context); // add security headers to the response
                if (IsRequestAllawed(context)==false) // spam request
                {
                    context.Response.StatusCode = (int)HttpStatusCode.TooManyRequests;
                    context.Response.ContentType = "application/json";
                    var response = new APIExceptions((int)HttpStatusCode.TooManyRequests, "Too many requests. Please try again later.");
                    await context.Response.WriteAsJsonAsync(response);
                }
                await _next(context);
            }
            catch (Exception ex)
            {
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                context.Response.ContentType = "application/json";

                var response =_environment.IsDevelopment()? new APIExceptions((int)HttpStatusCode.InternalServerError, ex.Message, ex.StackTrace?.ToString())
                    : new APIExceptions((int)HttpStatusCode.InternalServerError, ex.Message);

                var json = JsonSerializer.Serialize(response);
                await context.Response.WriteAsync(json);
            }
        }


        //limit the number of requests from a single IP address
        public bool IsRequestAllawed(HttpContext context)
        {
            //check if the request is from a specific IP address by get IP address from the request
            var ip = context.Connection.RemoteIpAddress.ToString();
            var cacheKey = $"Rate:{ip}";
            var DateNaw = DateTime.UtcNow;

            //use memory cache to store casheKey and time to check if the request is allowed or spam
            var(timespan, count) = _memoryCache.GetOrCreate(cacheKey, entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = _rateLimitWindow;
                return (timespan: DateNaw, count: 0);
            });
            //timespan = last request that i recved

            if (DateNaw-timespan <_rateLimitWindow) //if true that mean that is not first requesr
            {
                if (count>25) //number of request >25
                {
                    return false; // Block request
                }
                _memoryCache.Set(cacheKey, (timespan, count + 1), _rateLimitWindow);

            }
            else
            {
                _memoryCache.Set(cacheKey, (DateNaw, 1), _rateLimitWindow);
            }
            return true;

        }

        public void ApplaySecurityHeaders(HttpContext context)
        {
            
            context.Response.Headers["X-Content-Type-Options"]="nosniff"; //prevent javascript files
            
            context.Response.Headers.Add("X-XSS-Protection", "1; mode=block"); //block xss
            context.Response.Headers.Add("X-Frame-Options", "DENY"); // prevent access to take frame( clickjacking )
            context.Response.Headers.Add("Referrer-Policy", "no-referrer");
            context.Response.Headers.Add("Permissions-Policy", "geolocation=(self), microphone=()");
        }
    }
}
