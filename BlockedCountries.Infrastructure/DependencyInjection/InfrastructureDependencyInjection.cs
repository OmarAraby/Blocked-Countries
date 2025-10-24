using BlockedCountries.Application.Interfaces;
using BlockedCountries.Domain.Interfaces.Repositories;
using BlockedCountries.Infrastructure.Repositories.InMemory;
using BlockedCountries.Infrastructure.Services.Geolocation;
using Microsoft.Extensions.DependencyInjection;

namespace BlockedCountries.Infrastructure.DependencyInjection
{
    public static class InfrastructureDependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, Microsoft.Extensions.Configuration.ConfigurationManager configuration)
        {
            services.AddSingleton<IBlockedCountriesRepository, BlockedCountriesRepository>();
            services.AddSingleton<ITemporalBlocksRepository, TemporalBlocksRepository>();
            services.AddSingleton<IBlockedAttemptsLogRepository, BlockedAttemptsLogRepository>();

            // 
            services.AddHttpClient<IGeolocationService, GeolocationService>(client =>
            {
                client.BaseAddress = new Uri("https://api.ipgeolocation.io/");
            });

            return services;
        }
    }
}