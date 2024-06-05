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

// Add configuration settings
builder.Services.AddSingleton<IConfiguration>(builder.Configuration);

// Configure CORS to allow requests from the frontend
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        builder => builder.WithOrigins("http://localhost:3000", "https://localhost:3000")
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

// Bind to the port provided by Vercel
var port = Environment.GetEnvironmentVariable("PORT") ?? "5000";
app.Urls.Add($"http://*:{port}");

app.Run();
