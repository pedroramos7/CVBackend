using CVBackend.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "CV Website API", Version = "v1" });
});

// Register your repository for dependency injection
builder.Services.AddScoped<PersonalInfoRepository>();

// Assuming you have configuration settings like connection strings in your appsettings.json
// You might want to inject IConfiguration to access these settings in your repository
// This is done automatically by ASP.NET Core's dependency injection

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

