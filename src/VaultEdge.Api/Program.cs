using ErrorOr;
using VaultEdge.Api.Http;
using VaultEdge.Application;
using VaultEdge.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddApplication()
    .AddInfrastructure(builder.Configuration);

// Add Controllers & Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Cross-Origin Resource Sharing(CORS)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowVaultEdgeFrontend",
        builder =>
        {
            builder.WithOrigins("http://localhost:3000")
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
});

builder.Services.AddProblemDetails(options =>
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

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseExceptionHandler(errorApp =>
//{
//    errorApp.Run(async (context) =>
//    {
//        var exception = context.Features.Get<IExceptionHandlerFeature>()?.Error;

//        var (statusCode, message) = exception switch
//        {
//            IServiceException serviceException => ((int)serviceException.StatusCode, serviceException.ErrorMessage),
//            _ => (StatusCodes.Status500InternalServerError, "An unexpected error occurred.")
//        };

//        context.Response.StatusCode = statusCode;
//        context.Response.ContentType = "application/json";
//        await context.Response.WriteAsJsonAsync(new ProblemDetails
//        {
//            Status = statusCode,
//            Title = message
//        });
//    });
//});

app.UseExceptionHandler();
app.UseStatusCodePages();

app.UseCors("AllowVaultEdgeFrontend");
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();