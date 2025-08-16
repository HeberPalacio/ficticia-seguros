using FicticiaBackend.Controllers;
using FicticiaBackend.Data;
using FicticiaBackend.DTOs;
using FicticiaBackend.Models;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Xunit;

namespace FicticiaSA.Backend.Tests.Controllers
{
    public class AuthControllerTests
    {
        private FicticiaContext GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<FicticiaContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            return new FicticiaContext(options);
        }

        [Fact]
        public async Task Login_ShouldReturnUnauthorized_WhenUserNotFound()
        {
            var context = GetInMemoryDbContext();
            var config = new ConfigurationBuilder().AddInMemoryCollection().Build();
            var controller = new AuthController(context, config);

            var result = await controller.Login(new LoginDto
            {
                Username = "usuario_inexistente",
                Password = "1234"
            });

            result.Should().BeOfType<UnauthorizedObjectResult>();
        }

        [Fact]
        public async Task Login_ShouldReturnOk_WhenPasswordCorrect()
        {
            var context = GetInMemoryDbContext();

            // Crear usuario de prueba
            var user = new Usuario
            {
                Username = "admin",
            };
            user.PasswordHash = new PasswordHasher<Usuario>().HashPassword(user, "1234");
            context.Usuarios.Add(user);
            await context.SaveChangesAsync();

            var jwtData = new Dictionary<string, string?>
            {
                { "Jwt:Key", "EstaEsUnaClaveSuperSecretaMuyLarga1234" },
                { "Jwt:Issuer", "FicticiaBackend" },
                { "Jwt:Audience", "FicticiaBackendUsers" },
                { "Jwt:ExpireMinutes", "60" }
            };
            var config = new ConfigurationBuilder()
                .AddInMemoryCollection(jwtData.ToList()) 
                .Build();

            var controller = new AuthController(context, config);

            var result = await controller.Login(new LoginDto
            {
                Username = "admin",
                Password = "1234"
            });

            result.Should().BeOfType<OkObjectResult>();
        }
    }
}
