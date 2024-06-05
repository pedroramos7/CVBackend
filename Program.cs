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
builder.Services.AddScoped<AboutmeRepository>();
builder.Services.AddScoped<WorkExperienceRepository>();
builder.Services.AddScoped<CvRepository>();

// Assuming you have configuration settings like connection strings in your appsettings.json
// You might want to inject IConfiguration to access these settings in your repository
// This is done automatically by ASP.NET Core's dependency injection

// Configure CORS to allow requests from the frontend
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        builder => builder.WithOrigins("http://localhost:3000")
                          .AllowAnyMethod()
                          .AllowAnyHeader());
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowFrontend");  // Use the CORS policy
app.UseAuthorization();
app.MapControllers();
app.Run();

