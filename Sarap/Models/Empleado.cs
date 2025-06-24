using Sarap.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sarap.Models
{
    public class Empleado
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio")]
        [StringLength(50)]
        public string Nombre { get; set; } = string.Empty;

        [Required(ErrorMessage = "Los apellidos son obligatorios")]
        [StringLength(100)]
        public string Apellidos { get; set; } = string.Empty;

        [Required(ErrorMessage = "La cédula es obligatoria")]
        [StringLength(20)]
        public string Cedula { get; set; } = string.Empty;

        [EmailAddress(ErrorMessage = "Ingrese un email válido")]
        [StringLength(100)]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Seleccione un rol")]
        [StringLength(50)]
        public string Rol { get; set; } = string.Empty;

        public bool Activo { get; set; } = true;

        [ForeignKey("Usuario")]
        public int? UsuarioId { get; set; }
        public virtual Usuario Usuario { get; set; } = null!;
    }
}