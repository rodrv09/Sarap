using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Repository;
using Sarap.Models;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Collections.Generic;

public class AccountController : Controller
{
    private readonly UsuarioRepository _usuarioRepo;

    public AccountController(UsuarioRepository usuarioRepo)
    {
        _usuarioRepo = usuarioRepo;
    }

    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(RegistroViewModel model)
    {
        if (ModelState.IsValid)
        {
            if (await _usuarioRepo.UserExists(model.NombreUsuario, model.Email))
            {
                ModelState.AddModelError("", "El nombre de usuario o correo electrónico ya está registrado.");
                return View(model);
            }

            var usuario = new Usuario
            {
                Nombre = model.Nombre,
                Apellido = model.Apellido,
                NombreUsuario = model.NombreUsuario,
                Email = model.Email,
                Telefono = model.Telefono,
                ContraseñaHash = UsuarioRepository.HashPassword(model.Password),
                // FechaCreacion, Rol, Activo se asignan en el repositorio si quieres
            };

            var result = await _usuarioRepo.CreateAsync(usuario);

            if (result)
            {
                return RedirectToAction("Login", "Account");
            }

            ModelState.AddModelError("", "Error al crear el usuario. Por favor intente nuevamente.");
        }
        return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

        Response.Headers["Cache-Control"] = "no-cache, no-store, must-revalidate";
        Response.Headers["Pragma"] = "no-cache";
        Response.Headers["Expires"] = "0";

        return RedirectToAction("Login");
    }
    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(string username, string password)
    {
        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
        {
            ModelState.AddModelError("", "Debe ingresar usuario Y contraseña.");
            return View();
        }

        var usuario = await _usuarioRepo.GetByUsernameAsync(username);
        if (usuario == null)
        {
            ModelState.AddModelError("", "Usuario o contraseña Incorrectos.");
            return View();
        }

        bool validPassword = UsuarioRepository.VerifyPassword(password, usuario.ContraseñaHash);
        if (!validPassword)
        {
            ModelState.AddModelError("", "Usuario o contraseña incorrectos.");
            return View();
        }

        var claims = new List<Claim>
    {
        new Claim(ClaimTypes.Name, usuario.NombreUsuario),
        new Claim(ClaimTypes.Email, usuario.Email),
        new Claim(ClaimTypes.Role, usuario.Rol ?? "Usuario")
    };

        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var principal = new ClaimsPrincipal(identity);

        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

        return RedirectToAction("Index", "Home");
    }
    [HttpGet]
    public async Task<IActionResult> Cuenta()
    {
        if (!User.Identity.IsAuthenticated)
        {
            // Si no está logueado, lo redirigimos al login
            return RedirectToAction("Login");
        }

        // Obtenemos el nombre de usuario actual desde los claims
        var nombreUsuario = User.Identity.Name;

        // Buscamos el usuario en la base de datos
        var usuario = await _usuarioRepo.GetByUsernameAsync(nombreUsuario);

        if (usuario == null)
        {
            return RedirectToAction("Login");
        }

        // Mapeamos a tu ViewModel
        var model = new RegistroViewModel
        {
            Nombre = usuario.Nombre,
            Apellido = usuario.Apellido,
            NombreUsuario = usuario.NombreUsuario,
            Email = usuario.Email,
            Telefono = usuario.Telefono
        };

        return View(model);
    }
}