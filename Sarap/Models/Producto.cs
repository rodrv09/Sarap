namespace Sarap.Models
{
    public partial class Producto
    {
        public int ProductoId { get; set; }
        public string Nombre { get; set; } = null!;
        public int Cantidad { get; set; }
        public string? Descripcion { get; set; }
        public decimal Precio { get; set; }
        public DateTime FechaRegistro { get; set; }
    }
}
