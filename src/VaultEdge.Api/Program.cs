using Microsoft.EntityFrameworkCore;
using VaultEdge.Domain.Repositories;
using VaultEdge.Infrastructure.Persistence;
using VaultEdge.Infrastructure.Persistence.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add DB Context
builder.Services.AddDbContext<VaultEdgeDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddMediatR(configuration =>
{
    configuration.RegisterServicesFromAssembly(typeof(Program).Assembly);
});

// Add Controllers & Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add custom services (Application Layer)
builder.Services.AddScoped<IAccountRepository, AccountRepository>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
