namespace Sarap.Models
{
    public class PlanillaColones
{
    public int Id { get; set; }
    public string Identidad { get; set; }
    public DateTime FechaInicio { get; set; }
    public DateTime FechaFin { get; set; }
    public decimal SalarioOrdinario { get; set; }
    public decimal SalarioIncapacidad { get; set; }
    public decimal SalarioVacaciones { get; set; }
    public decimal SalarioFeriado { get; set; }
    public decimal SalarioExtra15 { get; set; }
    public decimal SalarioExtra20 { get; set; }
    public decimal SalarioBruto { get; set; }
    public decimal DeduccionCCSS { get; set; }
    public decimal SalarioNeto { get; set; }
    public DateTime? FechaRegistro { get; set; }            // Nullable
    public decimal HorasIncapacidad { get; set; }
    public decimal HorasVacaciones { get; set; }
    public decimal HorasFeriadoLey { get; set; }
    public decimal HorasExtra15 { get; set; }
    public decimal HorasExtra20 { get; set; }
    public decimal HorasPermisoSinGoce { get; set; }
    public decimal? HorasOrdinarias { get; set; }            // Nullable
}
}