using Microsoft.AspNetCore.Mvc;
using Repository; // tus repositorios
using System.Threading.Tasks;
using System.Linq;

namespace Sarap.Controllers
{
    public class HomeController : Controller
    {
        private readonly ClienteRepository _clienteRepository;
        private readonly ProveedorRepository _proveedorRepository;
        private readonly UsuarioRepository _usuarioRepository;
        private readonly ProductoRepository _productoRepository;
        private readonly EmpleadoRepository _empleadoRepository;
        private readonly RegistroHorasQuincenaRepository _registroHorasQuincenaRepository;
        private readonly PlanillaColonesRepository _planillaColonesRepository;
        private readonly VacacionesEmpleadoRepository _vacacionesEmpleadoRepository;
        private readonly FacturaRepository _facturaRepository;
        private readonly FacturaDetalleRepository _facturaDetalleRepository;

        public HomeController()
        {
            _clienteRepository = new ClienteRepository();
            _proveedorRepository = new ProveedorRepository();
            _usuarioRepository = new UsuarioRepository();
            _productoRepository = new ProductoRepository();
            _empleadoRepository = new EmpleadoRepository();
            _registroHorasQuincenaRepository = new RegistroHorasQuincenaRepository();
            _planillaColonesRepository = new PlanillaColonesRepository();
            _vacacionesEmpleadoRepository = new VacacionesEmpleadoRepository();
            _facturaRepository = new FacturaRepository();
            _facturaDetalleRepository = new FacturaDetalleRepository();
        }

        public async Task<IActionResult> Index()
        {
            var clientes = await _clienteRepository.ReadAsync();
            var proveedores = await _proveedorRepository.ReadAsync();
            var usuarios = await _usuarioRepository.ReadAsync();
            var productos = await _productoRepository.ReadAsync();
            var empleados = await _empleadoRepository.ReadAsync();
            var registroHorasQuincena = await _registroHorasQuincenaRepository.ReadAsync();
            var planillaColones = await _planillaColonesRepository.ReadAsync();
            var vacacionesEmpleado = await _vacacionesEmpleadoRepository.ReadAsync();
            var facturas = await _facturaRepository.ReadAsync();
            var facturaDetalles = await _facturaDetalleRepository.ReadAsync();

            ViewBag.TotalClientes = clientes.Count();
            ViewBag.TotalProveedores = proveedores.Count();
            ViewBag.TotalUsuarios = usuarios.Count();
            ViewBag.TotalProductos = productos.Count();
            ViewBag.TotalEmpleados = empleados.Count();
            ViewBag.TotalRegistroHorasQuincena = registroHorasQuincena.Count();
            ViewBag.TotalPlanillaColones = planillaColones.Count();
            ViewBag.TotalVacacionesEmpleado = vacacionesEmpleado.Count();
            ViewBag.TotalFacturas = facturas.Count();
            ViewBag.TotalFacturaDetalles = facturaDetalles.Count();

            return View();
        }
    }
}
