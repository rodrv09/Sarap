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

        public HomeController()
        {
            _clienteRepository = new ClienteRepository();
            _proveedorRepository = new ProveedorRepository();
            _usuarioRepository = new UsuarioRepository();
        }

        public async Task<IActionResult> Index()
        {
            var clientes = await _clienteRepository.ReadAsync();
            var proveedores = await _proveedorRepository.ReadAsync();
            var usuarios = await _usuarioRepository.ReadAsync();

            ViewBag.TotalClientes = clientes.Count();
            ViewBag.TotalProveedores = proveedores.Count();
            ViewBag.TotalUsuarios = usuarios.Count();

            return View();
        }
    }
}
