using Microsoft.AspNetCore.Mvc;
using Repository;
using Sarap.Models;
using System.Linq;
using System.Threading.Tasks;

namespace Sarap.Controllers
{
    public class PlanillaColonesController : Controller
    {
        private readonly PlanillaColonesRepository _repository;

        public PlanillaColonesController()
        {
            _repository = new PlanillaColonesRepository();
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var planillas = await _repository.ReadAsync();
                return View(planillas.ToList());
            }
            catch (Exception ex)
            {
                string errorCompleto = ex.Message;

                if (ex.InnerException != null)
                    errorCompleto += " | Inner: " + ex.InnerException.Message;

                if (ex.InnerException?.InnerException != null)
                    errorCompleto += " | Inner 2: " + ex.InnerException.InnerException.Message;

                TempData["Error"] = "Error al leer planillas: " + errorCompleto;

                return View(new List<PlanillaColones>());
            }

        }


        public IActionResult Crear()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Crear(PlanillaColones planilla)
        {
            if (!ModelState.IsValid)
                return View(planilla);

            var creado = await _repository.CreateAsync(planilla);
            if (creado)
            {
                TempData["Mensaje"] = "Planilla creada correctamente.";
                return RedirectToAction(nameof(Index));
            }

            ModelState.AddModelError("", "No se pudo crear la planilla.");
            return View(planilla);
        }

        public async Task<IActionResult> Editar(int id)
        {
            var planillas = await _repository.ReadAsync();
            var planilla = planillas.FirstOrDefault(p => p.Id == id);

            if (planilla == null)
                return NotFound();

            return View(planilla);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editar(PlanillaColones planilla)
        {
            if (!ModelState.IsValid)
                return View(planilla);

            var actualizado = await _repository.UpdateAsync(planilla);
            if (actualizado)
            {
                TempData["Mensaje"] = "Planilla actualizada correctamente.";
                return RedirectToAction(nameof(Index));
            }

            ModelState.AddModelError("", "No se pudo actualizar la planilla.");
            return View(planilla);
        }

        public async Task<IActionResult> Eliminar(int id)
        {
            var planillas = await _repository.ReadAsync();
            var planilla = planillas.FirstOrDefault(p => p.Id == id);

            if (planilla == null)
                return NotFound();

            return View(planilla);
        }

        [HttpPost, ActionName("Eliminar")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EliminarConfirmado(int id)
        {
            var planillas = await _repository.ReadAsync();
            var planilla = planillas.FirstOrDefault(p => p.Id == id);

            if (planilla == null)
                return NotFound();

            var eliminado = await _repository.DeleteAsync(planilla);
            if (eliminado)
            {
                TempData["Mensaje"] = "Planilla eliminada correctamente.";
                return RedirectToAction(nameof(Index));
            }

            ModelState.AddModelError("", "No se pudo eliminar la planilla.");
            return View(planilla);
        }
    }
}
