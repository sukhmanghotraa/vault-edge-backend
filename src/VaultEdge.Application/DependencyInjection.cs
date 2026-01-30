using Microsoft.Extensions.DependencyInjection;
using VaultEdge.Application.Authentication;

namespace VaultEdge.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        var assembly = typeof(DependencyInjection).Assembly;
        
        services.AddMediatR(configuration => 
            configuration.RegisterServicesFromAssembly(assembly));
        services.AddScoped<IAuthenticationService, AuthenticationService>();

        return services;
    }
}
