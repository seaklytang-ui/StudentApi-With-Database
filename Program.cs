using Asp.Versioning;
using FluentValidation; // condition insert data
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;//Save error when process
using StudentApi.Data;
using StudentApi.Interfaces;
using StudentApi.Middleware;
using StudentApi.Models;
using StudentApi.Repositories;
using StudentApi.Services;
using StudentApi.Validators;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
// version api
builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ReportApiVersions = true;
});

// Add services
builder.Services.AddControllers();

//Upload file 
builder.Services.AddScoped<IFileService, FileService>();

//// code jwt // Add Auth to swagger
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

                ValidIssuer = builder.Configuration["Jwt:Issuer"],
                IssuerSigningKey = 
                    new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(
                            builder.Configuration["Jwt:Key"]))
            };
    });

builder.Services.AddAuthorization();// enable authorization

// auto  save when error pel process
Log.Logger = new LoggerConfiguration()
    .WriteTo.File("logs/log.txt",
        rollingInterval: RollingInterval.Day)
    .CreateLogger();
builder.Host.UseSerilog();
//



builder.Services.AddScoped<IStudentRepository, StudentRepository>(); // Dependancy Injection (DI) create object auto

builder.Services.AddEndpointsApiExplorer();


builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Student API"
    });

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT"
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

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


//upload file head
app.UseStaticFiles();

app.UseAuthentication(); // enable authentication
app.UseAuthorization(); // enable authorization

app.UseMiddleware<ExceptionMiddleware>();// exception error 500 Internal Server

app.MapControllers();

app.Run();

public partial class Program { } // for test integration