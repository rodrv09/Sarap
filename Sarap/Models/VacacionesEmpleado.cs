namespace Sarap.Models
{
    public class VacacionesEmpleado
    {
        public int Id { get; set; }
        public string Identidad { get; set; }
        public string NombreCompleto { get; set; }
        public int DiasDisponibles { get; set; }
        public int DiasUsados { get; set; }
        public DateTime? FechaUltimaActualizacion { get; set; }
    }
}
