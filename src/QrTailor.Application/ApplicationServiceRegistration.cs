using QrTailor.Application.Behaviors;
using QrTailor.Application.Features.Auth.Queries;
using QrTailor.Application.Services.CrawlService.AmazonService;
using QrTailor.Application.Services.CrawlService.TrendyolService;
using QrTailor.Application.Services.EmailService;
using QrTailor.Application.Services.Utils;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Reflection;

namespace QrTailor.Application
{
    public static class ApplicationServiceRegistration
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
            services.AddValidatorsFromAssemblies(AppDomain.CurrentDomain.GetAssemblies());

            services.AddScoped<IEmailService, EmailManager>();
            services.AddScoped<IUtilsService, UtilsManager>();
            services.AddScoped<IAmazonCrawlService, AmazonCrawlManager>();
            services.AddScoped<ITrendyolCrawlService, TrendyolCrawlManager>();

            return services;

        }
    }
}
