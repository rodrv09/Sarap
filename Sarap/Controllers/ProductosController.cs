using Microsoft.AspNetCore.Mvc;
using Repository;
using Sarap.Models;


namespace Sarap.Controllers
{
    public class ProductosController : Controller
    {
        private readonly ProductoRepository _repository;

        public ProductosController()
        {
            _repository = new ProductoRepository();
        }

        public async Task<IActionResult> Index()
        {
            var productos = await _repository.ReadAsync();
            return View(productos.ToList());
        }

        public IActionResult Crear()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Crear(Producto producto)
        {
            if (!ModelState.IsValid)
                return View(producto);

            var creado = await _repository.CreateAsync(producto);
            if (creado)
            {
                TempData["Mensaje"] = "Producto creado correctamente.";
                return RedirectToAction(nameof(Index));
            }

            ModelState.AddModelError("", "No se pudo crear el producto.");
            return View(producto);
        }

        public async Task<IActionResult> Editar(int id)
        {
            var productos = await _repository.ReadAsync();
            var producto = productos.FirstOrDefault(p => p.ProductoId == id);

            if (producto == null)
                return NotFound();

            return View(producto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editar(Producto producto)
        {
            if (!ModelState.IsValid)
                return View(producto);

            var actualizado = await _repository.UpdateAsync(producto);
            if (actualizado)
            {
                TempData["Mensaje"] = "Producto actualizado correctamente.";
                return RedirectToAction(nameof(Index));
            }

            ModelState.AddModelError("", "No se pudo actualizar el producto.");
            return View(producto);
        }

        public async Task<IActionResult> Eliminar(int id)
        {
            var productos = await _repository.ReadAsync();
            var producto = productos.FirstOrDefault(p => p.ProductoId == id);

            if (producto == null)
                return NotFound();

            return View(producto);
        }

        [HttpPost, ActionName("Eliminar")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EliminarConfirmado(int id)
        {
            var productos = await _repository.ReadAsync();
            var producto = productos.FirstOrDefault(p => p.ProductoId == id);

            if (producto == null)
                return NotFound();

            var eliminado = await _repository.DeleteAsync(producto);
            if (eliminado)
            {
                TempData["Mensaje"] = "Producto eliminado correctamente.";
                return RedirectToAction(nameof(Index));
            }

            ModelState.AddModelError("", "No se pudo eliminar el producto.");
            return View(producto);
        }

    }
}