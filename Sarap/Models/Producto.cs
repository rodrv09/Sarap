namespace Sarap.Models
{
    public class Producto
    {
        public int ProductoID { get; set; }

        public string Nombre { get; set; }

        public string? Descripcion { get; set; }

        public string? Categoria { get; set; }

        public string? UnidadMedida { get; set; }

        public decimal Cantidad { get; set; }

        public decimal Precio { get; set; }

        public int? ProveedorID { get; set; }

        public bool? Activo { get; set; }

        public int? StockMinimo { get; set; }

        // Si tienes navegación a proveedor, podrías incluir:
        // public Proveedor? Proveedor { get; set; }
    }

}
