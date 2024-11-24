using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using QrTailor.Persistance.Context;
using QrTailor.Domain.Entities;

namespace QrTailor.Persistance
{
    public static class PersistenceServiceRegistration
    {
        public static IServiceCollection AddPersistenceServices(this IServiceCollection services,
                                                                IConfiguration configuration)
        {
            string myConnStr = ApplicationSettings.DbConnString;
            services.AddDbContext<DatabaseContext>(options => options.UseMySql(myConnStr,
                                                                            ServerVersion.AutoDetect(myConnStr)));
            //services.AddScoped<IBrandRepository, BrandRepository>();
            //services.AddScoped<IModelRepository, ModelRepository>();
            //services.AddScoped<IUserRepository, UserRepository>();
            //services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
            //services.AddScoped<IOperationClaimRepository, OperationClaimRepository>();
            //services.AddScoped<IUserOperationClaimRepository, UserOperationClaimRepository>();

            return services;
        }
    }
}
