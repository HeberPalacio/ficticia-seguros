using FicticiaBackend.Commands;
using FicticiaBackend.Data;
using FicticiaBackend.Queries;
using FicticiaBackend.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// 1. Configurar DbContext con SQL Server
builder.Services.AddDbContext<FicticiaContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// 2. Registrar repositorio
builder.Services.AddScoped<IPersonaRepository, PersonaRepository>();

builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.InvalidModelStateResponseFactory = context =>
    {
        var errores = context.ModelState
            .Where(x => x.Value.Errors.Count > 0)
            .Select(x => new
            {
                Campo = x.Key,
                Errores = x.Value.Errors.Select(e => e.ErrorMessage)
            });

        return new BadRequestObjectResult(new
        {
            success = false,
            message = "Datos inválidos",
            errores
        });
    };
});

builder.Services.AddControllers();

// 3. Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// 4. Registrar Queries y Commands
builder.Services.AddScoped<GetAllPersonasQuery>();
builder.Services.AddScoped<GetPersonaByIdQuery>();
builder.Services.AddScoped<CreatePersonaCommand>();
builder.Services.AddScoped<UpdatePersonaCommand>();
builder.Services.AddScoped<DeletePersonaCommand>();

// 5. Configuración de Autenticación JWT
var key = builder.Configuration["Jwt:Key"];
var issuer = builder.Configuration["Jwt:Issuer"];
var audience = builder.Configuration["Jwt:Audience"];

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = issuer,
            ValidAudience = audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key))
        };
    });

// 6. Configuración CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularDev",
        builder =>
        {
            builder.WithOrigins("http://localhost:4200")
                   .AllowAnyHeader()
                   .AllowAnyMethod();
        });
});

var app = builder.Build();

// Middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// 7. Usar CORS **antes** de Authentication/Authorization
app.UseCors("AllowAngularDev");

// Autenticación y Autorización
app.UseAuthentication();
app.UseMiddleware<FicticiaBackend.Middlewares.ErrorHandlingMiddleware>();
app.UseAuthorization();
app.MapControllers();

app.Run();
