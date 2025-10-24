using Microsoft.Extensions.DependencyInjection;
using FluentValidation;
using BlockedCountries.Application.Services;
using BlockedCountries.Application.Interfaces;
using BlockedCountries.Application.BackgroundServices;

namespace BlockedCountries.Application.DependencyInjection
{
    public static class ApplicationDependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddScoped<ICountryService, CountryService>();
            services.AddScoped<ITemporalBlockService, TemporalBlockService>();
            services.AddScoped<IBlockedAttemptsLogger, BlockedAttemptsLogger>();


            // background service 
            services.AddHostedService<TemporalBlockCleanupService>();

            // validations
            services.AddValidatorsFromAssembly(typeof(ApplicationDependencyInjection).Assembly);

            return services;
        }
    }
}
