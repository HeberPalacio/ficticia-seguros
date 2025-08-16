using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace FicticiaBackend.Models
{
    public class LoginRequest
    {
        [JsonPropertyName("usuario")]
        [Required(ErrorMessage = "El usuario es obligatorio")]
        public required string Username { get; set; }

        [JsonPropertyName("password")]
        [Required(ErrorMessage = "La contraseña es obligatoria")]
        public required string PasswordHash { get; set; }
    }
}
