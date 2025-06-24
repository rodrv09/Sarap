using Microsoft.AspNetCore.Mvc;
using Sarap.Repository;

namespace Sarap.Controllers
{
    public class HomeController : Controller
    {
        private readonly IClienteRepository _clienteRepository;
        private readonly IProveedorRepository _proveedorRepository;
        private readonly IUsuarioRepository _usuarioRepository;

        public HomeController(
            IClienteRepository clienteRepository,
            IProveedorRepository proveedorRepository,
            IUsuarioRepository usuarioRepository)
        {
            _clienteRepository = clienteRepository;
            _proveedorRepository = proveedorRepository;
            _usuarioRepository = usuarioRepository;
        }

        public async Task<IActionResult> Index()
        {
            var clientes = await _clienteRepository.GetAllAsync();
            var proveedores = await _proveedorRepository.GetAllAsync();
            var usuarios = await _usuarioRepository.GetAllAsync();

            ViewBag.TotalClientes = clientes.Count();
            ViewBag.TotalProveedores = proveedores.Count();
            ViewBag.TotalUsuarios = usuarios.Count();

            return View();
        }
    }
}
