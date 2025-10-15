using Hello.Domain.Interfaces;
using Hello.Domain.Interfaces.Repo;
using Hello.Domain.Models;
using Hello.Persistence.Repos;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace Hello.Persistence.Extensions
{
    public static class ServicesCollection
    {
        public static IServiceCollection AddPersistance(this IServiceCollection services, IConfiguration configuration)
        {
            services
                .AddContext(configuration)
                .AddRedis(configuration)
                .AddCollections();

            return services;
        }

        static IServiceCollection AddContext(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DataBase");

            services.AddDbContext<HelloDbContext>(options =>
               options.UseLazyLoadingProxies().UseSqlServer(connectionString,
                   builder => builder.MigrationsAssembly(typeof(HelloDbContext).Assembly.FullName)));

            // Identity configuration
            services.AddIdentity<User, IdentityRole>()
            .AddEntityFrameworkStores<HelloDbContext>()
                    .AddUserManager<UserManager<User>>()
                    .AddRoleManager<RoleManager<IdentityRole>>()
                    .AddSignInManager<SignInManager<User>>()
                    .AddSignInManager()
                    .AddDefaultTokenProviders();


            return services;
        }

        static IServiceCollection AddRedis(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IConnectionMultiplexer>(
                ConnectionMultiplexer.Connect(configuration["Redis"]!));

            services.AddSingleton<IUserNameBloomServices, UserNameBloomServices>();
           
            return services;
        }

        static IServiceCollection AddCollections(this IServiceCollection services)
        {
            services
                .AddScoped<IUnitOfWork, UnitOfWork>()
                .AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));

            return services;
        }
    }
}
