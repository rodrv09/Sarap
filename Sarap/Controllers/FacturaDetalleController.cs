using Microsoft.AspNetCore.Mvc;
using Repository;
using Sarap.Models;
using System.Linq;
using System.Threading.Tasks;

namespace Sarap.Controllers
{
    public class DetalleFacturaController : Controller
    {
        private readonly FacturaDetalleRepository _repository;

        public DetalleFacturaController()
        {
            _repository = new FacturaDetalleRepository();
        }

        // GET: DetalleFactura/Index/5  (5 = FacturaID)
        public async Task<IActionResult> Index(int facturaId)
        {
            var detalles = await _repository.ReadAsync();
            var detallesFactura = detalles.Where(d => d.FacturaID == facturaId).ToList();
            ViewBag.FacturaID = facturaId;
            return View(detallesFactura);
        }

        // GET: DetalleFactura/Crear?facturaId=5
        public IActionResult Crear(int facturaId)
        {
            var detalle = new FacturaDetalle { FacturaID = facturaId };
            return View(detalle);
        }

        // POST: DetalleFactura/Crear
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Crear(FacturaDetalle detalle)
        {
            if (!ModelState.IsValid)
                return View(detalle);

            var creado = await _repository.CreateAsync(detalle);
            if (creado)
            {
                TempData["Mensaje"] = "Detalle creado correctamente.";
                return RedirectToAction(nameof(Index), new { facturaId = detalle.FacturaID });
            }

            ModelState.AddModelError("", "No se pudo crear el detalle.");
            return View(detalle);
        }

        // GET: DetalleFactura/Editar/5
        public async Task<IActionResult> Editar(int id)
        {
            var detalles = await _repository.ReadAsync();
            var detalle = detalles.FirstOrDefault(d => d.DetalleID == id);
            if (detalle == null)
                return NotFound();

            return View(detalle);
        }

        // POST: DetalleFactura/Editar/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editar(FacturaDetalle detalle)
        {
            if (!ModelState.IsValid)
                return View(detalle);

            var actualizado = await _repository.UpdateAsync(detalle);
            if (actualizado)
            {
                TempData["Mensaje"] = "Detalle actualizado correctamente.";
                return RedirectToAction(nameof(Index), new { facturaId = detalle.FacturaID });
            }

            ModelState.AddModelError("", "No se pudo actualizar el detalle.");
            return View(detalle);
        }

        // GET: DetalleFactura/Eliminar/5
        public async Task<IActionResult> Eliminar(int id)
        {
            var detalles = await _repository.ReadAsync();
            var detalle = detalles.FirstOrDefault(d => d.DetalleID == id);
            if (detalle == null)
                return NotFound();

            return View(detalle);
        }

        // POST: DetalleFactura/Eliminar/5
        [HttpPost, ActionName("Eliminar")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EliminarConfirmado(int id)
        {
            var detalles = await _repository.ReadAsync();
            var detalle = detalles.FirstOrDefault(d => d.DetalleID == id);
            if (detalle == null)
                return NotFound();

            var eliminado = await _repository.DeleteAsync(detalle);
            if (eliminado)
            {
                TempData["Mensaje"] = "Detalle eliminado correctamente.";
                return RedirectToAction(nameof(Index), new { facturaId = detalle.FacturaID });
            }

            ModelState.AddModelError("", "No se pudo eliminar el detalle.");
            return View(detalle);
        }
    }
}
