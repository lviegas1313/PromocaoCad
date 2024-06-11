using CadastroAPI.Context;
using CadastroAPI.Filters;
using CadastroAPI.Models;
using CadastroAPI.Repositories;
using CadastroAPI.Validators;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.Json;
using CadastroAPI.Services;
using CadastroAPI.ModelBinders;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using CadastroAPI.ModelBinders.CadastroAPI.ModelBinders;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;

// Add services to the container.
services.AddControllers();

// Add DbContext with SQL Server
services.AddDbContext<CadastroContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add repositories
services.AddScoped<IUserRepository, UserRepository>();
services.AddScoped<INotaFiscalRepository, NotaFiscalRepository>();
services.AddScoped<INumerosSorteRepository, NumerosSorteRepository>();
services.AddScoped<IDatabaseRepository, DatabaseRepository>();

services.AddTransient<IEmailService, EmailService>();
services.AddMemoryCache();

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
        Description = "API de Cadastro de Usuários"
    });

    // Define o esquema de segurança JWT
    var securityScheme = new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme."
    };

    c.AddSecurityDefinition("Bearer", securityScheme);

    // Define os requisitos de segurança globais
    var securityRequirement = new OpenApiSecurityRequirement
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
            new string[] {}
        }
    };

    c.AddSecurityRequirement(securityRequirement);

    c.SchemaFilter<CorrectArraySchemaFilter>(); // Adiciona o SchemaFilter
    c.OperationFilter<FileUploadOperationFilter>();
});

services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
    });

services.AddControllersWithViews(options =>
{
    options.ModelBinderProviders.Insert(0, new NotaFiscalDTOBinderProvider());

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
app.UseCors("AllowSpecificOrigin");
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// Aplicar migrações e verificar/criar a procedure ao iniciar a aplicação de forma assíncrona
using (var scope = app.Services.CreateScope())
{
    var dbRepository = scope.ServiceProvider.GetRequiredService<IDatabaseRepository>();


    var configuration = builder.Configuration.GetSection("TruncateTablesOrder");
    // var configuration = config.Build();
    var tableNames = configuration.GetSection("TruncateTablesOrder").Get<List<string>>();

    //await dbRepository.TruncateTablesAsync(tableNames);

    // Aplicar migrações
    await dbRepository.ApplyMigrationsAsync();

    // Verificar e criar/alterar a procedure se necessário a partir do arquivo SQL
    var sqlFilePath = Path.Combine(AppContext.BaseDirectory, "YourProcedureName.sql");
    await dbRepository.EnsureProcedureExistsFromFileAsync(sqlFilePath);
}

app.Run();
