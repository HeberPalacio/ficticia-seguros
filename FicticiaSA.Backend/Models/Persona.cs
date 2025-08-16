using System.ComponentModel.DataAnnotations;

namespace FicticiaBackend.Models
{
    public class Persona
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre completo es obligatorio")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "El nombre debe tener entre 3 y 100 caracteres")]
        [RegularExpression(@"^(?!\s*$).+", ErrorMessage = "El nombre no puede estar vacío o solo contener espacios")]
        [Display(Name = "Nombre Completo")]
        public string NombreCompleto { get; set; } = string.Empty;

        [Required(ErrorMessage = "La identificación es obligatoria")]
        [StringLength(20, ErrorMessage = "La identificación no puede tener más de 20 caracteres")]
        [RegularExpression(@"^(?!\s*$).+", ErrorMessage = "La identificación no puede estar vacía o solo contener espacios")]
        public string Identificacion { get; set; } = string.Empty;

        [Range(0, 120, ErrorMessage = "La edad debe estar entre 0 y 120")]
        public int Edad { get; set; }

        [Required(ErrorMessage = "El género es obligatorio")]
        [RegularExpression("Masculino|Femenino|Otro", ErrorMessage = "Género inválido")]
        public string Genero { get; set; } = string.Empty;

        public bool EstadoActivo { get; set; }

        public bool Maneja { get; set; }
        public bool UsaLentes { get; set; }
        public bool Diabetico { get; set; }

        [MaxLength(200, ErrorMessage = "El texto de enfermedad debe tener máximo 200 caracteres")]
        public string? EnfermedadOtra { get; set; }
    }
}