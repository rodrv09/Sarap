using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sarap.Models
{
    [Table("Empleado")]
    public class Empleado
    {
        public int Id { get; set; }                  // NO null
        public string Nombre { get; set; }           // NO null
        public string Identidad { get; set; }        // NO null

        public string? Direccion { get; set; }       // SÍ null (nullable)
        public string? Telefono { get; set; }        // SÍ null
        public string? Email { get; set; }           // SÍ null

        public DateTime? FechaContratacion { get; set; } // SÍ null (nullable)
        public int? DiasVacacionesDisponibles { get; set; } // SÍ null (nullable)

        public string? ApellidoPaterno { get; set; }   // SÍ null (nullable)
        public string? ApellidoMaterno { get; set; }   // SÍ null (nullable)

        public decimal SalarioHora { get; set; }     // NO null
        public bool Activo { get; set; }              // NO null
    }


}
