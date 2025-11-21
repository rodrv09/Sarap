namespace Sarap.Models
{
    public class FacturaPaginadaViewModel
    {
        public List<Factura> Facturas { get; set; }
        public int PaginaActual { get; set; }
        public int TotalPaginas { get; set; }
    }

}
