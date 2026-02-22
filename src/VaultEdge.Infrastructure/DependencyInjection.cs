using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using VaultEdge.Application.Common.Interfaces.Authentication;
using VaultEdge.Application.Common.Interfaces.Persistence;
using VaultEdge.Application.Common.Interfaces.Services;
using VaultEdge.Application.Repositories;
using VaultEdge.Infrastructure.Identity;
using VaultEdge.Infrastructure.Persistence;
using VaultEdge.Infrastructure.Persistence.Repositories;
using VaultEdge.Infrastructure.Services;

namespace VaultEdge.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, ConfigurationManager configuration)
    {
        //services.AddSingleton<IUserRepository, InMemoryUserRepository>();
        //services.AddSingleton<IAccountRepository, InMemoryAccountRepository>();

        services.AddDbContext<VaultEdgeDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

        services.AddAuth(configuration);
        services.AddSingleton<IDateTimeProvider, DateTimeProvider>();

        services.AddScoped<IAccountRepository, AccountRepository>();
        services.AddScoped<ICustomerRepository, CustomerRepository>();

        services.AddIdentity<ApplicationUser, IdentityRole<Guid>>()
            .AddEntityFrameworkStores<VaultEdgeDbContext>()
            .AddDefaultTokenProviders();
        services.AddScoped<IIdentityService, IdentityService>();


        return services;
    }

    

    public static IServiceCollection AddAuth(this IServiceCollection services, ConfigurationManager configuration)
    {

        var jwtSettings = new JwtSettings();
        configuration.Bind(JwtSettings.SectionName, jwtSettings);


        if (string.IsNullOrWhiteSpace(jwtSettings.SecretKey))
            throw new InvalidOperationException("JwtSettings:SecretKey is missing. Set it in user-secrets or environment variables.");


        services.AddSingleton(Options.Create(jwtSettings));
        services.AddSingleton<IJwtTokenGenerator, JwtTokenGenerator>();

        services.AddAuthentication(defaultScheme: JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options => options.TokenValidationParameters = new TokenValidationParameters
            {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSettings.Issuer,
                    ValidAudience = jwtSettings.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SecretKey))
            });

        return services;
    }
}
