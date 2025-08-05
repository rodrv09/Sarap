namespace Sarap.Models
{
    public class FacturaPaginadaViewModel
    {
        public List<Factura> Facturas { get; set; }
        public int PaginaActual { get; set; }
        public int TotalPaginas { get; set; }
    }

    public class CategoriaPaginadaViewModel
    {
        public List<Categoria> Categorias { get; set; }
        public int PaginaActual { get; set; }
        public int TotalPaginas { get; set; }
    }

    public class ProductoPaginadoViewModel
    {
        public List<Producto> Productos { get; set; }
        public int PaginaActual { get; set; }
        public int TotalPaginas { get; set; }
    }

    public class ProveedorPaginadoViewModel
    {
        public List<Proveedore> Proveedores { get; set; }
        public int PaginaActual { get; set; }
        public int TotalPaginas { get; set; }
    }

    public class ClientePaginadoViewModel
    {
        public List<Cliente> Clientes { get; set; }
        public int PaginaActual { get; set; }
        public int TotalPaginas { get; set; }
    }

    public class NotaCreditoPaginadaViewModel
    {
        public List<NotaCredito> NotasCredito { get; set; }
        public int PaginaActual { get; set; }
        public int TotalPaginas { get; set; }
    }
}