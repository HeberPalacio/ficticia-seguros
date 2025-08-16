using Microsoft.EntityFrameworkCore;
using FicticiaBackend.Models;

namespace FicticiaBackend.Data
{
    public class FicticiaContext : DbContext
    {
        public FicticiaContext(DbContextOptions<FicticiaContext> options) : base(options)
        {
        }

        public DbSet<Persona> Personas { get; set; }

        public DbSet<Usuario> Usuarios { get; set; }

    }

}
