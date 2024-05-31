using CadastroAPI.Context;
using CadastroAPI.Filters;
using CadastroAPI.Repositories;
using CadastroAPI.Validators;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;

// Add services to the container.
services.AddControllers();

// Add DbContext with SQL Server
services.AddDbContext<CadastroContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

//Add repositories
services.AddScoped<IUserRepository, UserRepository>();
services.AddScoped<INotaFiscalRepository, NotaFiscalRepository>();


// Add FluentValidation
services.AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<UserValidator>());
services.AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<NotaFiscalValidator>());
services.AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<ProdutoValidator>());

// Add Authentication
services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        var jwtSettings = builder.Configuration.GetSection("JwtSettings");
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings["Issuer"],
            ValidAudience = jwtSettings["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["SecretKey"])),
            ClockSkew = TimeSpan.Zero,
            LifetimeValidator = (before, expires, token, param) =>
            {
                return expires > DateTime.UtcNow;
            }
        };
    });

// Add Authorization
services.AddAuthorization();


// Add Swagger
services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "CadastroAPI",
        Version = "v1",
        Description = "API de Cadastro de Usuários",
        Contact = new OpenApiContact
        {
            Name = "Seu Nome",
            Email = "seu@email.com"
        }
    });
    c.OperationFilter<SwaggerFileOperationFilter>();
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "CadastroAPI v1");
});

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
