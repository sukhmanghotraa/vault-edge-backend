using VaultEdge.Api;
using VaultEdge.Application;
using VaultEdge.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddPresentation()
    .AddApplication()
    .AddInfrastructure(builder.Configuration);

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