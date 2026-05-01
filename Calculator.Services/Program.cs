using Calculator.DAL;
using Calculator.DAL.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// 1. Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// 2. Database Configuration
builder.Services.AddDbContext<CalculatorDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<ICalculatorRepository, CalculatorRepository>();

// 3. Define the CORS Policy
builder.Services.AddCors(options => {
    options.AddPolicy("FrontendPolicy", b =>
        b.WithOrigins("https://gouselazams.github.io") // Explicitly allow your hosted frontend
         .AllowAnyMethod()
         .AllowAnyHeader()
         .SetIsOriginAllowedToAllowWildcardSubdomains());
});

var app = builder.Build();

// 4. Configure the HTTP request pipeline.
// UseRouting is implicit in .NET 8, but UseCors MUST come before MapControllers
app.UseCors("FrontendPolicy");

// Allow Swagger in production if you want to test the hosted API
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();