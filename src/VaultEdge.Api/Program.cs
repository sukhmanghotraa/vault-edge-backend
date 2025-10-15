using Microsoft.EntityFrameworkCore;
using VaultEdge.Application.Interfaces;
using VaultEdge.Infrastructure.Persistence;
using VaultEdge.Application.Services;

var builder = WebApplication.CreateBuilder(args);

// Add DB Context
builder.Services.AddDbContext<VaultEdgeDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add Controllers & Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add custom services (Application Layer)
builder.Services.AddScoped<IAccountService, AccountService>();

// Register Services
builder.Services.AddScoped<AccountService>();

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
