using Hello.Domain.Interfaces;
using Hello.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Hello.Infrastructure.Extensions
{
    public static class ServicesCollection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services
                .AddServices();

            return services;
        }

        static IServiceCollection AddServices(this IServiceCollection services)
        {
            services
                .AddScoped<IAuthSerivces, AuthServices>()
                .AddScoped<IMediaServices, MediaServices>();

            return services;
        }
    }
}