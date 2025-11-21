using Microsoft.AspNetCore.Mvc;
using Repository;
using Sarap.Models;
using System.Linq;
using System.Threading.Tasks;

namespace Sarap.Controllers
{
    public class CategoriasController : Controller
    {
        private readonly CategoriaRepository _repository;

        public CategoriasController()
        {
            _repository = new CategoriaRepository();
        }

        // GET: Categorias
        public async Task<IActionResult> Index()
        {
            var categorias = await _repository.ReadAsync();
            return View(categorias.ToList());
        }

        // GET: Categorias/Crear
        public IActionResult Crear()
        {
            return View();
        }

        // POST: Categorias/Crear
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Crear(Categoria categoria)
        {
            if (!ModelState.IsValid)
                return View(categoria);

            var creado = await _repository.CreateAsync(categoria);
            if (creado)
            {
                TempData["Mensaje"] = "Categoría creada correctamente.";
                return RedirectToAction(nameof(Index));
            }

            ModelState.AddModelError("", "No se pudo crear la categoría.");
            return View(categoria);
        }

        // GET: Categorias/Editar/5
        public async Task<IActionResult> Editar(int id)
        {
            var categorias = await _repository.ReadAsync();
            var categoria = categorias.FirstOrDefault(c => c.CategoriaID == id);
            if (categoria == null)
                return NotFound();

            return View(categoria);
        }

        // POST: Categorias/Editar/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editar(Categoria categoria)
        {
            if (!ModelState.IsValid)
                return View(categoria);

            var actualizado = await _repository.UpdateAsync(categoria);
            if (actualizado)
            {
                TempData["Mensaje"] = "Categoría actualizada correctamente.";
                return RedirectToAction(nameof(Index));
            }

            ModelState.AddModelError("", "No se pudo actualizar la categoría.");
            return View(categoria);
        }

        // GET: Categorias/Eliminar/5
        public async Task<IActionResult> Eliminar(int id)
        {
            var categorias = await _repository.ReadAsync();
            var categoria = categorias.FirstOrDefault(c => c.CategoriaID == id);
            if (categoria == null)
                return NotFound();

            return View(categoria);
        }

        // POST: Categorias/Eliminar/5
        [HttpPost, ActionName("Eliminar")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EliminarConfirmado(int id)
        {
            var categorias = await _repository.ReadAsync();
            var categoria = categorias.FirstOrDefault(c => c.CategoriaID == id);
            if (categoria == null)
                return NotFound();

            var eliminado = await _repository.DeleteAsync(categoria);
            if (eliminado)
            {
                TempData["Mensaje"] = "Categoría eliminada correctamente.";
                return RedirectToAction(nameof(Index));
            }

            ModelState.AddModelError("", "No se pudo eliminar la categoría.");
            return View(categoria);
        }
    }
}
