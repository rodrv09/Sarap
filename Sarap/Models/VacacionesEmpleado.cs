namespace Sarap.Models
{
    public class VacacionesEmpleado
    {
        public int Id { get; set; }
        public string Identidad { get; set; }
        public string NombreCompleto { get; set; }

        public DateTime? FechaContratacion { get; set; } // Nullable porque en la BD permite null
        public int? DiasDisponibles { get; set; }        // Nullable porque en la BD permite null
        public int? DiasUsados { get; set; }             // Nullable porque en la BD permite null
        public DateTime? FechaUltimaActualizacion { get; set; }  // Nullable porque en la BD permite null
    }
}
