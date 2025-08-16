using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace FicticiaBackend.Models
{
    public class Usuario
    {
        public int Id { get; set; }

        [Required]
        public string Username { get; set; } = null!;

        [Required]
        public string PasswordHash { get; set; } = null!;

        public string Roles { get; set; } = string.Empty;
    }
}
