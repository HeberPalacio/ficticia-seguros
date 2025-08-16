namespace FicticiaBackend.DTOs
{
    public class LoginDto
    {
        public string Username { get; set; }   // Coincide con Usuario.Username
        public string Password { get; set; }   // Se compara con PasswordHash
    }
}