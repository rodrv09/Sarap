using Microsoft.AspNetCore.Mvc;
using Repository;
using Sarap.Models;
using System.Linq;
using System.Threading.Tasks;

namespace Sarap.Controllers
{
    public class EmpleadosController : Controller
    {
        private readonly EmpleadoRepository _repository;

        public EmpleadosController()
        {
            _repository = new EmpleadoRepository();
        }

        public async Task<IActionResult> Index()
        {
            var empleados = await _repository.ReadAsync();
            return View(empleados.ToList());
        }

        public IActionResult Crear()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Crear(Empleado empleado)
        {
            if (!ModelState.IsValid)
                return View(empleado);

            var creado = await _repository.CreateAsync(empleado);
            if (creado)
            {
                TempData["Mensaje"] = "Empleado creado correctamente.";
                return RedirectToAction(nameof(Index));
            }

            ModelState.AddModelError("", "No se pudo crear el empleado.");
            return View(empleado);
        }

        public async Task<IActionResult> Editar(int id)
        {
            var empleados = await _repository.ReadAsync();
            var empleado = empleados.FirstOrDefault(e => e.EmpleadoId == id);

            if (empleado == null)
                return NotFound();

            return View(empleado);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editar(Empleado empleado)
        {
            if (!ModelState.IsValid)
                return View(empleado);

            var actualizado = await _repository.UpdateAsync(empleado);
            if (actualizado)
            {
                TempData["Mensaje"] = "Empleado actualizado correctamente.";
                return RedirectToAction(nameof(Index));
            }

            ModelState.AddModelError("", "No se pudo actualizar el empleado.");
            return View(empleado);
        }

        public async Task<IActionResult> Eliminar(int id)
        {
            var empleados = await _repository.ReadAsync();
            var empleado = empleados.FirstOrDefault(e => e.EmpleadoId == id);

            if (empleado == null)
                return NotFound();

            return View(empleado);
        }

        [HttpPost, ActionName("Eliminar")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EliminarConfirmado(int id)
        {
            var empleados = await _repository.ReadAsync();
            var empleado = empleados.FirstOrDefault(e => e.EmpleadoId == id);

            if (empleado == null)
                return NotFound();

            var eliminado = await _repository.DeleteAsync(empleado);
            if (eliminado)
            {
                TempData["Mensaje"] = "Empleado eliminado correctamente.";
                return RedirectToAction(nameof(Index));
            }

            ModelState.AddModelError("", "No se pudo eliminar el empleado.");
            return View(empleado);
        }

        /// <summary>
        /// Activa un empleado.
        /// </summary>
        public async Task<IActionResult> Activar(int id)
        {
            var empleados = await _repository.ReadAsync();
            var empleado = empleados.FirstOrDefault(e => e.EmpleadoId == id);

            if (empleado == null)
                return NotFound();

            var activado = await _repository.ActivarAsync(empleado);
            if (activado)
            {
                TempData["Mensaje"] = "Empleado activado correctamente.";
            }
            else
            {
                TempData["Mensaje"] = "No se pudo activar el empleado.";
            }

            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// Desactiva un empleado.
        /// </summary>
        public async Task<IActionResult> Desactivar(int id)
        {
            var empleados = await _repository.ReadAsync();
            var empleado = empleados.FirstOrDefault(e => e.EmpleadoId == id);

            if (empleado == null)
                return NotFound();

            var desactivado = await _repository.DesactivarAsync(empleado);
            if (desactivado)
            {
                TempData["Mensaje"] = "Empleado desactivado correctamente.";
            }
            else
            {
                TempData["Mensaje"] = "No se pudo desactivar el empleado.";
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
