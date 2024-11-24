using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using QrTailor.Infrastructure.Errors.Middleware;
using QrTailor.Infrastructure.CustomExceptionFilter;
using QrTailor.Infrastructure.LogEntries;
using QrTailor.Infrastructure.Security.JwtToken;

namespace QrTailor.Infrastructure
{
    public static class InfrastructureServiceRegistration
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
        {

            services.AddScoped<ITokenHelper, JwtHelper>();
            services.AddScoped<ExceptionMiddleware>();

            services.AddTransient<LogFilter>();
            services.AddTransient<ExceptionFilter>();

            return services;

        }
    }
}
