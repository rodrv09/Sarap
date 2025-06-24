using System;
using System.Collections.Generic;

namespace Sarap.Models;

public partial class Proveedore
{
    public int ProveedorId { get; set; }

    public string NombreEmpresa { get; set; } = null!;

    public string ContactoNombre { get; set; } = null!;

    public string? Telefono { get; set; }

    public string? Email { get; set; }

    public string? Direccion { get; set; }

    public DateTime? FechaRegistro { get; set; }
    public bool Activo { get; set; }

}
