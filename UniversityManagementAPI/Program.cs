using Microsoft.EntityFrameworkCore;
using UniversityManagementAPI.Data;
using UniversityManagementAPI.Middleware;
using UniversityManagementAPI.Services; // Add this line for RabbitMQService

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<UniversityDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<TenantMiddleware>();
// Register RabbitMQ Service
builder.Services.AddSingleton<RabbitMQService>();

// Register HttpClient for communication with microservices
builder.Services.AddHttpClient();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddDbContext<UniversityDbContext>((serviceProvider, options) =>
{
    var tenant = serviceProvider.GetRequiredService<IHttpContextAccessor>()
        .HttpContext
        .Items["Tenant"]?.ToString() ?? "branch_1"; // Default to branch_1 if not found

    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();