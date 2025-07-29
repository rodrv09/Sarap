using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;

namespace Sarap.Models
{
    public class FacturaViewModel
    {
        public Factura Factura { get; set; }

        public FacturaDetalle NuevoDetalle { get; set; }

        public List<FacturaDetalle> FacturaDetalles { get; set; }

        public List<SelectListItem> ProductosSelectList { get; set; }
        public List<SelectListItem> ClientesSelectList { get; set; } = new List<SelectListItem>();

        public List<Factura> Facturas { get; set; }

    }
}
