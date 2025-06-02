using Microsoft.AspNetCore.Mvc;
using Repository;
using Sarap.Models;
using System.Linq;
using System.Threading.Tasks;

namespace Sarap.Controllers
{
    public class ClientesController : Controller
    {
        private readonly ClienteRepository _repository;

        public ClientesController()
        {
            _repository = new ClienteRepository();
        }

        public async Task<IActionResult> Index()
        {
            var clientes = await _repository.ReadAsync();
            return View(clientes.ToList());
        }

        public IActionResult Crear()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Crear(Cliente cliente)
        {
            if (!ModelState.IsValid)
                return View(cliente);

            var creado = await _repository.CreateAsync(cliente);
            if (creado)
            {
                TempData["Mensaje"] = "Cliente creado correctamente.";
                return RedirectToAction(nameof(Index));
            }

            ModelState.AddModelError("", "No se pudo crear el cliente.");
            return View(cliente);
        }

        public async Task<IActionResult> Editar(int id)
        {
            var clientes = await _repository.ReadAsync();
            var cliente = clientes.FirstOrDefault(c => c.ClienteId == id);

            if (cliente == null)
                return NotFound();

            return View(cliente);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editar(Cliente cliente)
        {
            if (!ModelState.IsValid)
                return View(cliente);

            var actualizado = await _repository.UpdateAsync(cliente);
            if (actualizado)
            {
                TempData["Mensaje"] = "Cliente actualizado correctamente.";
                return RedirectToAction(nameof(Index));
            }

            ModelState.AddModelError("", "No se pudo actualizar el cliente.");
            return View(cliente);
        }

        public async Task<IActionResult> Eliminar(int id)
        {
            var clientes = await _repository.ReadAsync();
            var cliente = clientes.FirstOrDefault(c => c.ClienteId == id);

            if (cliente == null)
                return NotFound();

            return View(cliente);
        }

        [HttpPost, ActionName("Eliminar")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EliminarConfirmado(int id)
        {
            var clientes = await _repository.ReadAsync();
            var cliente = clientes.FirstOrDefault(c => c.ClienteId == id);

            if (cliente == null)
                return NotFound();

            var eliminado = await _repository.DeleteAsync(cliente);
            if (eliminado)
            {
                TempData["Mensaje"] = "Cliente eliminado correctamente.";
                return RedirectToAction(nameof(Index));
            }

            ModelState.AddModelError("", "No se pudo eliminar el cliente.");
            return View(cliente);
        }


    }
}
