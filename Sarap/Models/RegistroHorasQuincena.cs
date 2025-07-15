namespace Sarap.Models
{
    public class RegistroHorasQuincena
    {
        public int Id { get; set; }

        public string Identidad { get; set; }  // varchar o nvarchar en BD, aquí string

        public DateTime? FechaInicio { get; set; }  // nullable si puede ser null en BD

        public DateTime? FechaFin { get; set; }

        public decimal HorasOrdinarias { get; set; }  // decimal para horas con decimales

        public decimal HorasIncapacidad { get; set; }

        public decimal HorasVacaciones { get; set; }

        public decimal HorasFeriadoLey { get; set; }

        public decimal HorasExtra15 { get; set; }

        public decimal HorasExtra20 { get; set; }

        public decimal HorasPermisoSinGoce { get; set; }

        public DateTime FechaRegistro { get; set; }
    }

}
