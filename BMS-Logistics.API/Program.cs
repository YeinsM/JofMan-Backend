using BMS_Logistics.API.Middlewares;
using BMS_Logistics.API.Services;
using BMS_Logistics.Application.Common.Authentication;
using BMS_Logistics.Application.Interfaces;
using BMS_Logistics.Infrastructure.Extensions;
using BMS_Logistics.Infrastructure.Persistence.Context;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using Swashbuckle.AspNetCore.SwaggerUI;
using System.Reflection;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// OpenAPI / Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(opt =>
{
    opt.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "BMS Logistics Backend",
        Version = "v1",
        Description = "En esta API podemos ver la documentación de Swagger del proyecto BMS Logistics",
        Contact = new OpenApiContact
        {
            Name = "TechBrains",
            Email = "business@techbrains.com.do"
        }
    });

    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    opt.IncludeXmlComments(xmlPath);

    opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Ingrese el token JWT en el campo: Bearer {token}",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        BearerFormat = "JWT",
        Scheme = "bearer"
    });

    //opt.AddSecurityRequirement(new OpenApiSecurityRequirement
    //{
    //    {
    //       new OpenApiSecurityScheme
    //        {
    //            Reference = new BaseOpenApiReference
    //            {
    //                Type = ReferenceType.SecurityScheme,
    //                Id = "Bearer"
    //            }
    //        },
    //        Array.Empty<string>()
    //    }
    //});
});
//builder.Services.AddSwaggerGen();

// Bind JwtSettings
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));
var jwtSettings = builder.Configuration.GetSection("JwtSettings").Get<JwtSettings>();

if (jwtSettings == null)
    throw new InvalidOperationException("Sección 'JwtSettings' no encontrada en la configuración. Añade la sección JwtSettings en appsettings o en Secret Manager.");
if (string.IsNullOrWhiteSpace(jwtSettings.SecretKey))
    throw new InvalidOperationException("JwtSettings:Key no está configurada. Proporcione una clave simétrica segura (use Secret Manager en producción).");

var jwtKeyBytes = Encoding.UTF8.GetBytes(jwtSettings.SecretKey);

// Register token service and map interfaces to same singleton instance
builder.Services.AddSingleton<JwtTokenService>();
builder.Services.AddSingleton<ITokenService>(sp => sp.GetRequiredService<JwtTokenService>());
builder.Services.AddSingleton<IJwtTokenGenerator>(sp => sp.GetRequiredService<JwtTokenService>());


var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<DataContext>(options => options.UseNpgsql(connectionString));

// Services
builder.Services.AddApplicationServices();

builder.Services.AddHttpContextAccessor();

// Configure Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = true;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings.Issuer,
        ValidAudience = jwtSettings.Audience,
        IssuerSigningKey = new SymmetricSecurityKey(jwtKeyBytes)
    };
});

builder.Services.AddAuthorization();

builder.Services.AddTransient<ExceptionHandlerMiddleware>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    // Habilitar Swagger UI en desarrollo y servirla en /scalar para permitir deep links
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "BMS Logistics API v1");
        // Serve Swagger UI at /scalar
        c.RoutePrefix = "scalar";
        // Opciones para una UI más compacta
        c.DocExpansion(DocExpansion.None);
        c.DefaultModelsExpandDepth(-1);
    });
}

app.UseMiddleware<ExceptionHandlerMiddleware>();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
