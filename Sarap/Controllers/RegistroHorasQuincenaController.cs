using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Repository;
using Sarap.Models;
using System.Linq;
using System.Threading.Tasks;

namespace Sarap.Controllers
{
    public class RegistroHorasQuincenaController : Controller
    {
        private readonly RegistroHorasQuincenaRepository _repository;

        public RegistroHorasQuincenaController()
        {
            _repository = new RegistroHorasQuincenaRepository();
        }

        // GET: RegistroHorasQuincena
        public async Task<IActionResult> Index()
        {
            var registros = await _repository.ReadAsync();

            using (var context = new EspeciasSarapiquiContext())
            {
                var empleados = await context.Empleados
                    .Select(e => new { e.Identidad, NombreCompleto = e.Nombre + " " + e.ApellidoPaterno + " " + e.ApellidoMaterno })
                    .ToListAsync();

                ViewBag.Empleados = empleados;
            }

            return View(registros.ToList());
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Crear(RegistroHorasQuincena registro)
        {
            if (!ModelState.IsValid)
                return View(registro);

            // Verifica si la identidad existe en la tabla Empleado
            using (var context = new EspeciasSarapiquiContext()) // Usa tu DbContext real
            {
                var existeEmpleado = await context.Empleados
                    .AnyAsync(e => e.Identidad == registro.Identidad);

                if (!existeEmpleado)
                {
                    ModelState.AddModelError("Identidad", "La identidad ingresada no existe en la base de datos.");
                    return View(registro);
                }
            }

            // Si todo bien, crea el registro
            var creado = await _repository.CreateAsync(registro);
            if (creado)
            {
                try
                {
                    using (var context = new EspeciasSarapiquiContext())
                    {
                        await context.Database.ExecuteSqlRawAsync(
                            "EXEC dbo.sp_GenerarPlanillaColones @p0, @p1, @p2",
                            registro.Identidad, registro.FechaInicio, registro.FechaFin
                        );
                    }

                    TempData["Mensaje"] = "Registro de horas y planilla generados correctamente.";
                }
                catch (Exception ex)
                {
                    TempData["Error"] = $"Registro creado, pero ocurrió un error al generar la planilla: {ex.Message}";
                }

                return RedirectToAction(nameof(Index));
            }

            ModelState.AddModelError("", "No se pudo crear el registro.");
            return View(registro);
        }



        // GET: RegistroHorasQuincena/Editar/5
        public async Task<IActionResult> Editar(int id)
        {
            var registros = await _repository.ReadAsync();
            var registro = registros.FirstOrDefault(r => r.Id == id);

            if (registro == null)
                return NotFound();

            return View(registro);
        }

        // POST: RegistroHorasQuincena/Editar
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editar(RegistroHorasQuincena registro)
        {
            if (!ModelState.IsValid)
                return View(registro);

            var actualizado = await _repository.UpdateAsync(registro);
            if (actualizado)
            {
                TempData["Mensaje"] = "Registro actualizado correctamente.";
                return RedirectToAction(nameof(Index));
            }

            ModelState.AddModelError("", "No se pudo actualizar el registro.");
            return View(registro);
        }

        // GET: RegistroHorasQuincena/Eliminar/5
        public async Task<IActionResult> Eliminar(int id)
        {
            var registros = await _repository.ReadAsync();
            var registro = registros.FirstOrDefault(r => r.Id == id);

            if (registro == null)
                return NotFound();

            return View(registro);
        }

        // POST: RegistroHorasQuincena/Eliminar/5
        [HttpPost, ActionName("Eliminar")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EliminarConfirmado(int id)
        {
            var registros = await _repository.ReadAsync();
            var registro = registros.FirstOrDefault(r => r.Id == id);

            if (registro == null)
                return NotFound();

            var eliminado = await _repository.DeleteAsync(registro);
            if (eliminado)
            {
                TempData["Mensaje"] = "Registro eliminado correctamente.";
                return RedirectToAction(nameof(Index));
            }

            ModelState.AddModelError("", "No se pudo eliminar el registro.");
            return View(registro);
        }
    }
}
