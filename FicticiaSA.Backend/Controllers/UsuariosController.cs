using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using FicticiaBackend.Data;
using FicticiaBackend.Models;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace FicticiaBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private readonly FicticiaContext _context;
        private readonly PasswordHasher<Usuario> _passwordHasher;

        public UsuariosController(FicticiaContext context)
        {
            _context = context;
            _passwordHasher = new PasswordHasher<Usuario>();
        }

        // POST: api/Usuarios
        [HttpPost]
        public async Task<IActionResult> CrearUsuario([FromBody] Usuario usuario)
        {
            // Validar campos obligatorios
            if (string.IsNullOrWhiteSpace(usuario.Username) ||
                string.IsNullOrWhiteSpace(usuario.PasswordHash))
            {
                return BadRequest(new { message = "Faltan campos obligatorios" });
            }

            // Verificar si el usuario ya existe
            if (await _context.Usuarios.AnyAsync(u => u.Username == usuario.Username))
            {
                return Conflict(new { message = "El usuario ya está registrado" });
            }

            // Hashear la contraseña
            usuario.PasswordHash = _passwordHasher.HashPassword(usuario, usuario.PasswordHash);

            // Guardar en la base de datos
            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(CrearUsuario), new { id = usuario.Id }, usuario);
        }
    }
}
