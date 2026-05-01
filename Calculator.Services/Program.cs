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
// Use a specific name like "FrontendPolicy" for better clarity
builder.Services.AddCors(options => {
    options.AddPolicy("FrontendPolicy", b =>
        b.AllowAnyMethod()
         .AllowAnyHeader()
         // When you host the backend, you can replace AllowAnyOrigin with your specific GitHub URL
         .AllowAnyOrigin());
});

var app = builder.Build();

// 4. Configure the HTTP request pipeline.
// IMPORTANT: UseCors must come BEFORE MapControllers and AFTER UseRouting (if present)
app.UseCors("FrontendPolicy");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();