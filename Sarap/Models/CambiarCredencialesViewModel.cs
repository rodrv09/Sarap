using System.ComponentModel.DataAnnotations;

namespace Sarap.Models
{
    public class CambiarCredencialesViewModel
    {
        [Required(ErrorMessage = "Debe ingresar el nombre de usuario.")]
        [Display(Name = "Nombre de Usuario")]
        public string NombreUsuario { get; set; } = null!;

        [Required(ErrorMessage = "El correo es obligatorio.")]
        [EmailAddress(ErrorMessage = "Debe ingresar un correo electrónico válido.")]
        [Display(Name = "Nuevo Correo Electrónico")]
        public string NuevoEmail { get; set; } = null!;

        [Required(ErrorMessage = "Debe ingresar la contraseña actual.")]
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña Actual")]
        public string ContraseñaActual { get; set; } = null!;

        [Required(ErrorMessage = "Debe ingresar una nueva contraseña.")]
        [DataType(DataType.Password)]
        [Display(Name = "Nueva Contraseña")]
        public string NuevaContraseña { get; set; } = null!;

        [Required(ErrorMessage = "Debe confirmar la nueva contraseña.")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirmar Nueva Contraseña")]
        [Compare("NuevaContraseña", ErrorMessage = "Las contraseñas no coinciden.")]
        public string ConfirmarContraseña { get; set; } = null!;
    }
}