using System.ComponentModel.DataAnnotations.Schema;
namespace Sarap.Models
{
    [Table("Empleado")]
    public class Empleado
    {
        public int EmpleadoId { get; set; }
        public string Nombre { get; set; }
        public string Identidad { get; set; }
        public string Email { get; set; }
        public string Telefono { get; set; }
        public string Direccion { get; set; }
        public DateTime FechaContratacion { get; set; }
        public int DiasVacacionesDisponibles { get; set; }
    }
}