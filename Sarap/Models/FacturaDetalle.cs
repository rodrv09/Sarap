using System.ComponentModel.DataAnnotations.Schema;

namespace Sarap.Models
{
    [Table("FacturaDetalles")]

    public class FacturaDetalle
    {
        public int DetalleID { get; set; }

        public int FacturaID { get; set; }

        public int ProductoID { get; set; }


        public string NombreProducto { get; set; }


        public decimal Cantidad { get; set; }

        public decimal PrecioUnitario { get; set; }


        public decimal Subtotal { get; set; }

        public decimal? Impuesto { get; set; }  // Nullable

        public decimal? Descuento { get; set; }  // Nullable

        public decimal TotalLinea { get; set; }
    }

}

