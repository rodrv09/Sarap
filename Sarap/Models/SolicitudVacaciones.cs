public class SolicitudVacaciones
{
    public int Id { get; set; }
    public int EmpleadoId { get; set; }
    public DateTime FechaSolicitud { get; set; } = DateTime.Now;
    public int DiasSolicitados { get; set; }
    public DateTime FechaInicio { get; set; }
    public string Comentarios { get; set; }
    public string Estado { get; set; } = "Pendiente"; // Pendiente/Aprobada/Rechazada
}