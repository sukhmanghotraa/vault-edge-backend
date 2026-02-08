using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using VaultEdge.Application.Authentication.Commands.Signup;
using VaultEdge.Application.Authentication.Queries.Signin;
using VaultEdge.Application.Common.Behaviors;

namespace VaultEdge.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        var assembly = typeof(DependencyInjection).Assembly;
        
        services.AddMediatR(configuration => 
            configuration.RegisterServicesFromAssembly(assembly));

        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

        services.AddScoped<IValidator<SignupCommand>, SignupCommandValidator>();
        services.AddScoped<IValidator<SigninQuery>, SigninQueryValidator>();

        return services;
    }
}