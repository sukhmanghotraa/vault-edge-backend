using ErrorOr;
using VaultEdge.Api.Common.Mapping;
using VaultEdge.Api.Http;

namespace VaultEdge.Api;

public static class DependencyInjection
{
    public static IServiceCollection AddPresentation(this IServiceCollection services)
    {
        
        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();

        services.AddCors(options =>
        {
            options.AddPolicy("AllowVaultEdgeFrontend",
                builder =>
                {
                    builder.WithOrigins("http://localhost:3000")
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
        });

        services.AddProblemDetails(options =>
        {
            options.CustomizeProblemDetails = context =>
            {
                var errors = context.HttpContext.Items[HttpContextItemKeys.Errors] as List<Error>;
                if (errors is not null)
                {
                    context.ProblemDetails.Extensions.Add("errorCodes", errors.Select(e => e.Code));
                }
            };
        });

        //builder.Services.AddSingleton<ProblemDetailsFactory, VaultEdgeProblemDetailsFactory>();

        services.AddMappings();

        return services;
    }
}