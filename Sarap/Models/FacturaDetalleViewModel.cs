using System.Collections.Generic;

namespace Sarap.Models
{
    public class FacturaDetalleViewModel
    {
        public Factura Factura { get; set; }
        public List<FacturaDetalle> Detalles { get; set; }
    }
}
