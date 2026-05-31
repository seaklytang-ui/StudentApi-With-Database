using Microsoft.EntityFrameworkCore;
using StudentApi.Data;
using StudentApi.Interfaces;
using StudentApi.Models;
using StudentApi.Repositories;
using StudentApi.Services;
using StudentApi.Middleware;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using FluentValidation; // condition insert data
using FluentValidation.AspNetCore;
using StudentApi.Validators;
using Serilog;//Save error when process


var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddControllers();


//// code jwt
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters =
            new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = false,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,

                ValidIssuer = builder.Configuration["jwt:Issuer"],
                IssuerSigningKey =
                    new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(
                            builder.Configuration["jwt:Key"]))
            };
    });
// auto  save when error pel process
Log.Logger = new LoggerConfiguration()
    .WriteTo.File("logs/log.txt",
        rollingInterval: RollingInterval.Day)
    .CreateLogger();
builder.Host.UseSerilog();
//

builder.Services.AddScoped<IStudentRepository, StudentRepository>(); // Dependancy Injection (DI) create object auto

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IStudentService,StudentService>();

// AuthToken
builder.Services.AddScoped<IAuthService,AuthService>();

// exception insert data
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<StudentValidator>();

// Database
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// Configure pipeline
app.UseSwagger();

app.UseSwaggerUI();

//app.UseAuthorization();
app.UseAuthentication(); // enable authentication
app.UseMiddleware<ExceptionMiddleware>();// exception error 500 Internal Server

app.MapControllers();

app.Run();