using Microsoft.EntityFrameworkCore;
using StudentApi.Data;
using StudentApi.Interfaces;
using StudentApi.Models;
using StudentApi.Repositories;
using StudentApi.Services;
using StudentApi.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddControllers();
builder.Services.AddScoped<IStudentRepository, StudentRepository>(); // Dependancy Injection (DI) create object auto

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IStudentService,StudentService>();

// Database
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// Configure pipeline
app.UseSwagger();

app.UseSwaggerUI();

//app.UseAuthorization();
app.UseMiddleware<ExceptionMiddleware>();// exception error 500 Internal Server

app.MapControllers();

app.Run();