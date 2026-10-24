using API.Modules;
using Application.Service;
using DB;
using DB.Repositories;
using DB.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString));

builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<IServiceRepository, ServiceRepository>();
builder.Services.AddScoped<ITransactionRepository, TransactionRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ISnapshotRepository, SnapshotRepository>();
builder.Services.AddSingleton<ITokenCreator, JWTCreator>();

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ISnapshotService, SnapshotService>();

builder.SetupJWT();
builder.Services.AddControllers();

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}


app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.MapSwagger();
app.UseSwaggerUI(c =>
{
    c.RoutePrefix = "api/v1/docs"; 
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "API V1");
});

await app.SeedUsersAsync();

app.Run();
