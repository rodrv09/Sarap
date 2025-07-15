using Microsoft.AspNetCore.Mvc;
using Repository;
using Sarap.Models;
using System.Linq;
using System.Threading.Tasks;

namespace Sarap.Controllers
{
    public class ProductosController : Controller
    {
        private readonly ProductoRepository _repository;
        private readonly CategoriaRepository _categoriaRepository;
        private readonly ProveedorRepository _proveedorRepository;

        public ProductosController()
        {
            _repository = new ProductoRepository();
            _categoriaRepository = new CategoriaRepository();
            _proveedorRepository = new ProveedorRepository();
        }

        public async Task<IActionResult> Index()
        {
            var productos = await _repository.ReadAsync();
            var categorias = await _categoriaRepository.ReadAsync();
            var proveedores = await _proveedorRepository.ReadAsync();

            ViewBag.Categorias = categorias.ToList();
            ViewBag.Proveedores = proveedores.ToList();

            return View(productos.ToList());
        }

    // Mostrar formulario para crear
    public IActionResult Crear()
        {
            // Simplemente redirige al Index si no hay vista Crear.cshtml
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Crear(Producto producto)
        {
            if (!ModelState.IsValid)
            {
                var errores = ModelState.Values
                                .SelectMany(v => v.Errors)
                                .Select(e => e.ErrorMessage)
                                .ToList();

                TempData["Error"] = "Errores: " + string.Join(", ", errores);
                return RedirectToAction(nameof(Index));
            }

            var creado = await _repository.CreateAsync(producto);
            if (creado)
            {
                TempData["Mensaje"] = "Producto creado correctamente.";
                return RedirectToAction(nameof(Index));
            }

            TempData["Error"] = "No se pudo crear el producto.";
            return RedirectToAction(nameof(Index));
        }




        // Mostrar formulario de edición
        public async Task<IActionResult> Editar(int id)
        {
            var productos = await _repository.ReadAsync();
            var producto = productos.FirstOrDefault(p => p.ProductoID == id);

            if (producto == null)
                return NotFound();

            return View(producto);
        }

        // Guardar cambios en la edición
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

        // Confirmación para eliminar
        public async Task<IActionResult> Eliminar(int id)
        {
            var productos = await _repository.ReadAsync();
            var producto = productos.FirstOrDefault(p => p.ProductoID == id);

            if (producto == null)
                return NotFound();

            return View(producto);
        }

        // Eliminar definitivamente
        [HttpPost, ActionName("Eliminar")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EliminarConfirmado(int id)
        {
            var productos = await _repository.ReadAsync();
            var producto = productos.FirstOrDefault(p => p.ProductoID == id);

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
