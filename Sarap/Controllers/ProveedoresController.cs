using Microsoft.AspNetCore.Mvc;
using Repository;
using Sarap.Models;
using System.Linq;
using System.Threading.Tasks;

namespace Sarap.Controllers
{
    public class ProveedoresController : Controller
    {
        private readonly ProveedorRepository _repository;

        public ProveedoresController()
        {
            _repository = new ProveedorRepository();
        }

        public async Task<IActionResult> Index()
        {
            var proveedores = await _repository.ReadAsync();
            return View(proveedores.ToList());
        }

        public IActionResult Crear()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Crear(Proveedore proveedor)
        {
            if (!ModelState.IsValid)
                return View(proveedor);

            var creado = await _repository.CreateAsync(proveedor);
            if (creado)
            {
                TempData["Mensaje"] = "Proveedor creado correctamente.";
                return RedirectToAction(nameof(Index));
            }

            ModelState.AddModelError("", "No se pudo crear el proveedor.");
            return View(proveedor);
        }

        public async Task<IActionResult> Editar(int id)
        {
            var proveedores = await _repository.ReadAsync();
            var proveedor = proveedores.FirstOrDefault(p => p.ProveedorId == id);

            if (proveedor == null)
                return NotFound();

            return View(proveedor);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editar(Proveedore proveedor)
        {
            if (!ModelState.IsValid)
                return View(proveedor);

            var actualizado = await _repository.UpdateAsync(proveedor);
            if (actualizado)
            {
                TempData["Mensaje"] = "Proveedor actualizado correctamente.";
                return RedirectToAction(nameof(Index));
            }

            ModelState.AddModelError("", "No se pudo actualizar el proveedor.");
            return View(proveedor);
        }

        public async Task<IActionResult> Eliminar(int id)
        {
            var proveedores = await _repository.ReadAsync();
            var proveedor = proveedores.FirstOrDefault(p => p.ProveedorId == id);

            if (proveedor == null)
                return NotFound();

            return View(proveedor);
        }

        [HttpPost, ActionName("Eliminar")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EliminarConfirmado(int id)
        {
            var proveedores = await _repository.ReadAsync();
            var proveedor = proveedores.FirstOrDefault(p => p.ProveedorId == id);

            if (proveedor == null)
                return NotFound();

            var eliminado = await _repository.DeleteAsync(proveedor);
            if (eliminado)
            {
                TempData["Mensaje"] = "Proveedor eliminado correctamente.";
                return RedirectToAction(nameof(Index));
            }

            ModelState.AddModelError("", "No se pudo eliminar el proveedor.");
            return View(proveedor);
        }
    }
}
