using Microsoft.OpenApi.Models;
using backend.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "AMI Assessment API", Version = "v1" });
});
builder.Services.AddHttpClient();
builder.Services.AddScoped<WeatherService>();

// CORS, for development purpose only, remove for final submission
builder.Services.AddCors(options =>
{
    options.AddPolicy("DevCorsPolicy", builder =>
    {
        builder.WithOrigins("http://localhost:5173")
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

var app = builder.Build();

// Warning: This is used to avoid CORS, it basically runs the frontend built code
// app.UseDefaultFiles();
// app.UseStaticFiles();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseCors("DevCorsPolicy");
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "AMI Assessment API v1");
    });
}

app.UseAuthorization();
app.MapControllers();
app.Run();
