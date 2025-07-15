using System.ComponentModel.DataAnnotations;

namespace Sarap.Models
{
    public class CambiarCredencialesViewModel
    {
        [Required]
        [EmailAddress]
        public string NuevoEmail { get; set; } = null!;

        [Required]
        [DataType(DataType.Password)]
        public string ContraseñaActual { get; set; } = null!;

        [Required]
        [DataType(DataType.Password)]
        public string NuevaContraseña { get; set; } = null!;

        [Required]
        [DataType(DataType.Password)]
        [Compare("NuevaContraseña", ErrorMessage = "Las contraseñas no coinciden.")]
        public string ConfirmarContraseña { get; set; } = null!;
    }
}
