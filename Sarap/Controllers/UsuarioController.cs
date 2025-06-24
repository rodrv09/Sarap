using Microsoft.AspNetCore.Mvc;
using Sarap.Models;
using Sarap.Repository;

namespace Sarap.Controllers
{
    public class UsuariosController : Controller
    {
        private readonly IUsuarioRepository _repository;

        public UsuariosController(IUsuarioRepository repository)
        {
            _repository = repository;
        }

        public async Task<IActionResult> Index()
        {
            var usuarios = await _repository.ReadAsync();
            return View(usuarios.ToList());
        }

        public async Task<IActionResult> Editar(int id)
        {
            var usuarios = await _repository.ReadAsync();
            var usuario = usuarios.FirstOrDefault(u => u.UsuarioId == id);

            if (usuario == null)
                return NotFound();

            return View(usuario);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editar(Usuario usuario)
        {
            if (!ModelState.IsValid)
            {
                var usuarios = await _repository.ReadAsync();
                return View("Index", usuarios.ToList());
            }

            var actualizado = await _repository.UpdateAsync(usuario);
            if (actualizado)
            {
                TempData["Mensaje"] = "Usuario actualizado correctamente.";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                ModelState.AddModelError("", "No se pudo actualizar el usuario.");
                var usuarios = await _repository.ReadAsync();
                return View("Index", usuarios.ToList());
            }
        }

        public IActionResult Crear()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Crear(Usuario usuario)
        {
            if (!ModelState.IsValid)
                return View(usuario);

            var creado = await _repository.CreateAsync(usuario);
            if (creado)
            {
                TempData["Mensaje"] = "Usuario creado correctamente.";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                ModelState.AddModelError("", "No se pudo crear el usuario.");
                return View(usuario);
            }
        }

        public async Task<IActionResult> Eliminar(int id)
        {
            var usuarios = await _repository.ReadAsync();
            var usuario = usuarios.FirstOrDefault(u => u.UsuarioId == id);

            if (usuario == null)
                return NotFound();

            return View(usuario);
        }

        [HttpPost, ActionName("Eliminar")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EliminarConfirmado(int id)
        {
            var usuarios = await _repository.ReadAsync();
            var usuario = usuarios.FirstOrDefault(u => u.UsuarioId == id);

            if (usuario == null)
                return NotFound();

            var eliminado = await _repository.DeleteAsync(usuario);
            if (eliminado)
            {
                TempData["Mensaje"] = "Usuario eliminado correctamente.";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                ModelState.AddModelError("", "No se pudo eliminar el usuario.");
                return View(usuario);
            }
        }

        public async Task<IActionResult> Activar(int id)
        {
            var usuarios = await _repository.ReadAsync();
            var usuario = usuarios.FirstOrDefault(u => u.UsuarioId == id);

            if (usuario == null)
                return NotFound();

            var activado = await _repository.ActivarAsync(usuario);
            if (activado)
            {
                TempData["Mensaje"] = "Usuario activado correctamente.";
            }
            else
            {
                TempData["Mensaje"] = "No se pudo activar el usuario.";
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Desactivar(int id)
        {
            var usuarios = await _repository.ReadAsync();
            var usuario = usuarios.FirstOrDefault(u => u.UsuarioId == id);

            if (usuario == null)
                return NotFound();

            var desactivado = await _repository.DesactivarAsync(usuario);
            if (desactivado)
            {
                TempData["Mensaje"] = "Usuario desactivado correctamente.";
            }
            else
            {
                TempData["Mensaje"] = "No se pudo desactivar el usuario.";
            }

            return RedirectToAction(nameof(Index));
        }

    }
}
