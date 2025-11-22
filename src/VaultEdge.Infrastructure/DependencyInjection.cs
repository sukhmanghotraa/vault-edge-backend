using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VaultEdge.Application.Abstractions;
using VaultEdge.Domain.Repositories;
using VaultEdge.Infrastructure.Identity;
using VaultEdge.Infrastructure.Persistence;
using VaultEdge.Infrastructure.Persistence.Repositories;

namespace VaultEdge.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        //services.AddSingleton<IUserRepository, InMemoryUserRepository>();
        //services.AddSingleton<IAccountRepository, InMemoryAccountRepository>();
        services.AddDbContext<VaultEdgeDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

        services.AddScoped<IAccountRepository, AccountRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IJwtProvider, JwtProvider>();

        return services;
    }
}
