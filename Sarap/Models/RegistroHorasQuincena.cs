namespace Sarap.Models
{
    public class RegistroHorasQuincena
    {
        public int Id { get; set; }
        public string Identidad { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public decimal HorasOrdinarias { get; set; }
        public decimal HorasIncapacidad { get; set; }
        public decimal HorasVacaciones { get; set; }
        public decimal HorasFeriadoLey { get; set; }
        public decimal HorasExtra15 { get; set; }
        public decimal HorasExtra20 { get; set; }
        public decimal HorasPermisoSinGoce { get; set; }
        public DateTime FechaRegistro { get; set; }
    }
}
