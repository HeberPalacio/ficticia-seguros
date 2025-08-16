using FicticiaBackend.Data;
using FicticiaBackend.Models;
using FicticiaBackend.DTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace FicticiaBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly FicticiaContext _context;
        private readonly IConfiguration _config;
        private readonly PasswordHasher<Usuario> _passwordHasher;

        public AuthController(FicticiaContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
            _passwordHasher = new PasswordHasher<Usuario>();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto request)
        {
            // Validación básica
            if (string.IsNullOrWhiteSpace(request.Username) || string.IsNullOrWhiteSpace(request.Password))
                return BadRequest("Username y Password son requeridos");

            try
            {
                // Buscar usuario en la base de datos
                var usuario = await _context.Usuarios
                    .FirstOrDefaultAsync(u => u.Username == request.Username);

                if (usuario == null)
                    return Unauthorized("Usuario no encontrado");

                // Validar que PasswordHash no sea nulo
                if (string.IsNullOrEmpty(usuario.PasswordHash))
                    return Unauthorized("Contraseña no configurada para este usuario");

                // Verificar contraseña
                var resultado = _passwordHasher.VerifyHashedPassword(usuario, usuario.PasswordHash, request.Password);

                if (resultado == PasswordVerificationResult.Failed)
                    return Unauthorized("Contraseña incorrecta");

                // Generar JWT incluyendo roles
                var token = GenerateJwtToken(usuario);

                return Ok(new
                {
                    token,
                    roles = usuario.Roles // <-- enviar roles al frontend
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno: {ex.Message}");
            }
        }

        private string GenerateJwtToken(Usuario usuario)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, usuario.Username),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim("roles", string.Join(",", usuario.Roles)) // <-- roles en el token
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"] ?? "clave_secreta_temporal"));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var expireMinutes = int.Parse(_config["Jwt:ExpireMinutes"] ?? "60");

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(expireMinutes),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
