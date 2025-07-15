namespace Sarap.Models
{
  
    public class Factura
    {

        public int FacturaID { get; set; }

        public DateTime Fecha { get; set; }

        public string ClienteNombre { get; set; }  
        public string? ClienteIdentidad { get; set; }  // Nullable

        public decimal Total { get; set; }

        public decimal? Subtotal { get; set; }  // Nullable

        public decimal? Impuesto { get; set; }  // Nullable

        public decimal? Descuento { get; set; }  // Nullable
        public string TipoPago { get; set; }

        public string? Usuario { get; set; }  // Nullable

        public string? Observaciones { get; set; }  // nvarchar(max), nullable
    }

}
