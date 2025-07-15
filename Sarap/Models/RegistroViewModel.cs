using System.ComponentModel.DataAnnotations;

namespace Sarap.Models;

public class RegistroViewModel
{
    [Required(ErrorMessage = "Nombre es obligatorio")]
    public string Nombre { get; set; } = null!;

    [Required(ErrorMessage = "Apellido es obligatorio")]
    public string Apellido { get; set; } = null!;

    [Required(ErrorMessage = "Nombre de usuario es obligatorio")]
    public string NombreUsuario { get; set; } = null!;

    [Required(ErrorMessage = "Email es obligatorio")]
    [EmailAddress(ErrorMessage = "Formato de email inválido")]
    public string Email { get; set; } = null!;

    [Required(ErrorMessage = "Contraseña es obligatoria")]
    [DataType(DataType.Password)]
    [StringLength(100, MinimumLength = 8, ErrorMessage = "La contraseña debe tener al menos 8 caracteres")]
    public string Password { get; set; } = null!;

    [DataType(DataType.Password)]
    [Compare("Password", ErrorMessage = "Las contraseñas no coinciden")]
    public string ConfirmPassword { get; set; } = null!;

    public string? Telefono { get; set; }
}