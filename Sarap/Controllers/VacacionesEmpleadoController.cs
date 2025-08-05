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
        public async Task<IActionResult> Solicitar(
    [FromForm] int id,
    [FromForm] int cantidadDias,
    [FromForm] DateTime fechaInicio,
    [FromForm] string comentarios)
        {
            if (cantidadDias <= 0)
            {
                return Json(new { success = false, message = "La cantidad de días debe ser mayor a 0." });
            }

            var actualizado = await _repository.SolicitarDiasAsync(id, cantidadDias);

            if (actualizado)
            {
                // Aquí iría la lógica para guardar el historial de solicitud
                // con los datos adicionales (fechaInicio, comentarios, etc.)

                return Json(new
                {
                    success = true,
                    message = "Solicitud de vacaciones procesada correctamente."
                });
            }

            return Json(new
            {
                success = false,
                message = "No se pudieron solicitar los días de vacaciones. Verifique los días disponibles."
            });
        }
    }
}
