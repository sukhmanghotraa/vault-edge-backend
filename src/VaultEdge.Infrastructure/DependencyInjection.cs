using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VaultEdge.Application.Abstractions;
using VaultEdge.Application.Common.Interfaces.Authentication;
using VaultEdge.Application.Common.Interfaces.Services;
using VaultEdge.Domain.Repositories;
using VaultEdge.Infrastructure.Identity;
using VaultEdge.Infrastructure.Persistence;
using VaultEdge.Infrastructure.Persistence.Repositories;
using VaultEdge.Infrastructure.Services;

namespace VaultEdge.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        //services.AddSingleton<IUserRepository, InMemoryUserRepository>();
        //services.AddSingleton<IAccountRepository, InMemoryAccountRepository>();
        services.AddDbContext<VaultEdgeDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
        services.Configure<JwtSettings>(configuration.GetSection(JwtSettings.SectionName));
        services.AddScoped<IAccountRepository, AccountRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddSingleton<IJwtTokenGenerator, JwtTokenGenerator>();
        services.AddSingleton<IDateTimeProvider, DateTimeProvider>();


        return services;
    }
}
