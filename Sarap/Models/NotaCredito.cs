namespace Sarap.Models
{
    public class NotaCredito
    {
        public int NotaCreditoID { get; set; }
        public int FacturaID { get; set; }
        public DateTime Fecha { get; set; } = DateTime.Now;
        public string Motivo { get; set; } = string.Empty;
        public decimal Monto { get; set; }
    }

}
