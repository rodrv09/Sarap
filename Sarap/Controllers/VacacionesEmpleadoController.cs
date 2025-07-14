using Microsoft.AspNetCore.Mvc;
using Sarap.Models;
using Repository;
using System.Linq;
using System.Threading.Tasks;

namespace Sarap.Controllers
{
    public class VacacionesEmpleadoController : Controller
    {
        private readonly VacacionesEmpleadoRepository _repository;

        public VacacionesEmpleadoController()
        {
            _repository = new VacacionesEmpleadoRepository();
        }

        public async Task<IActionResult> Index()
        {
            var vacaciones = await _repository.ReadAsync();
            return View(vacaciones.ToList());
        }

        public async Task<IActionResult> Solicitar(int id)
        {
            var vacaciones = await _repository.GetByIdAsync(id);
            if (vacaciones == null)
                return NotFound();

            return View(vacaciones);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Solicitar(int id, int cantidadDias)
        {
            if (cantidadDias <= 0)
            {
                ModelState.AddModelError("", "La cantidad de días debe ser mayor a 0.");
                return View();
            }

            var actualizado = await _repository.SolicitarDiasAsync(id, cantidadDias);

            if (actualizado)
            {
                TempData["Mensaje"] = "Días de vacaciones solicitados correctamente.";
                return RedirectToAction(nameof(Index));
            }

            ModelState.AddModelError("", "No se pudieron solicitar los días de vacaciones.");
            return View();
        }
    }
}
