using Sarap.Models;
using Sarap.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Sarap.Controllers
{
    public class EmpleadosController : Controller
    {
        private readonly IEmpleadoRepository _empleadoRepository;
        private readonly IUsuarioRepository _usuarioRepository;
        public EmpleadosController(
            IEmpleadoRepository empleadoRepository,
            IUsuarioRepository usuarioRepository) 
        {
            _empleadoRepository = empleadoRepository;
            _usuarioRepository = usuarioRepository;
        }

        public async Task<IActionResult> Index(string searchTerm)
        {
            if (!string.IsNullOrEmpty(searchTerm))
            {
                return View(await _empleadoRepository.SearchAsync(searchTerm));
            }
            return View(await _empleadoRepository.GetAllAsync());
        }

        public async Task<IActionResult> Create()
        {
            await LoadDropdowns();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Empleado empleado)
        {
            if (ModelState.IsValid)
            {
                await _empleadoRepository.AddAsync(empleado);
                return RedirectToAction(nameof(Index));
            }

            await LoadDropdowns();
            return View(empleado);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var empleado = await _empleadoRepository.GetByIdAsync(id);
            if (empleado == null)
            {
                return NotFound();
            }

            await LoadDropdowns();
            return View(empleado);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Empleado empleado)
        {
            if (id != empleado.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await _empleadoRepository.UpdateAsync(empleado);
                return RedirectToAction(nameof(Index));
            }

            await LoadDropdowns();
            return View(empleado);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AssignRole(int id, string role)
        {
            await _empleadoRepository.AssignRoleAsync(id, role);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var empleado = await _empleadoRepository.GetByIdAsync(id);
            if (empleado == null)
            {
                return NotFound();
            }

            return View(empleado);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _empleadoRepository.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }

        private async Task LoadDropdowns()
        {
            var roles = new List<string> { "Administrador", "Cajero", "Bodeguero", "Vendedor", "Gerente" };
            ViewBag.Roles = new SelectList(roles);

            var usuarios = await _usuarioRepository.GetAllAsync();
            ViewBag.Usuarios = new SelectList(usuarios, "Id", "NombreUsuario");
        }
    }
}