using Microsoft.AspNetCore.Mvc;
using Repository;
using Sarap.Models;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Text;

namespace Sarap.Controllers
{
    public class UsuariosController : Controller
    {
        private readonly UsuarioRepository _repository;

        public UsuariosController()
        {
            _repository = new UsuarioRepository();
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

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Crear(Usuario usuario)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Datos inválidos al crear el usuario.";
                return RedirectToAction(nameof(Index));
            }

            var creado = await _repository.CreateAsync(usuario);
            if (creado)
            {
                TempData["Mensaje"] = "Usuario creado correctamente.";
            }
            else
            {
                TempData["Error"] = "No se pudo crear el usuario.";
            }

            return RedirectToAction(nameof(Index));
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
        public IActionResult CambiarCredenciales()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CambiarCredenciales(CambiarCredencialesViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var usuarios = await _repository.ReadAsync();
            var username = User.Identity?.Name;

            var usuario = usuarios.FirstOrDefault(u => u.NombreUsuario == username);

            if (usuario == null)
            {
                ModelState.AddModelError("", "Usuario no encontrado.");
                return View(model);
            }

            // Validar contraseña actual
            var contraseñaHashActual = HashPassword(model.ContraseñaActual);
            if (usuario.ContraseñaHash != contraseñaHashActual)
            {
                ModelState.AddModelError("ContraseñaActual", "La contraseña actual es incorrecta.");
                return View(model);
            }

            // Actualizar datos
            usuario.Email = model.NuevoEmail;
            usuario.ContraseñaHash = HashPassword(model.NuevaContraseña);

            var actualizado = await _repository.UpdateAsync(usuario);
            if (actualizado)
            {
                TempData["Mensaje"] = "Credenciales actualizadas correctamente.";
                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", "No se pudo actualizar la información.");
            return View(model);
        }

        // Función para hashear contraseña
        private string HashPassword(string password)
        {
            using var sha = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(password);
            var hash = sha.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }
    }
}
