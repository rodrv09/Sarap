using System;
using System.Collections.Generic;

namespace Sarap.Models;

public partial class Usuario
{
    public int UsuarioId { get; set; }
    public string Nombre { get; set; } = null!;
    public string Apellido { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string? Telefono { get; set; }
    public string NombreUsuario { get; set; } = null!;
    public string ContraseñaHash { get; set; } = null!; // Almacena el hash SHA256
    public DateTime? FechaCreacion { get; set; } = DateTime.Now;
    public string Rol { get; set; } = "Usuario";
    public bool Activo { get; set; } = true;
}
