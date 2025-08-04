using Microsoft.AspNetCore.Mvc;
using Sarap.Models;
using System.Linq;
using System;

namespace Sarap.Controllers
{
    public class NotaCreditoController : Controller
    {
        private readonly EspeciasSarapiquiContext _context;

        public NotaCreditoController(EspeciasSarapiquiContext context)
        {
            _context = context;
        }

        // GET: NotaCredito/Index
        public IActionResult Index()
        {
            var notas = _context.NotaCredito
                .OrderByDescending(n => n.Fecha)
                .ToList();
            return View(notas);
        }

        // GET: NotaCredito/Crear
        public IActionResult Crear()
        {
            // Redirigir a selección de factura o mostrar formulario con selector
            return RedirectToAction("Index", "Facturas"); // Asumiendo que hay un controlador de facturas
        }

        // GET: NotaCredito/Crear/{facturaId}
        public IActionResult CrearParaFactura(int facturaId)
        {
            var factura = _context.Facturas.FirstOrDefault(f => f.FacturaID == facturaId);
            if (factura == null)
            {
                return NotFound();
            }

            ViewBag.FacturaNumero = factura.FacturaID;
            ViewBag.FacturaCliente = factura.ClienteNombre;
            ViewBag.FacturaTotal = factura.Total;

            var nota = new NotaCredito
            {
                FacturaID = factura.FacturaID,
                Fecha = DateTime.Now // Establecer fecha actual por defecto
            };

            return View("Crear", nota);
        }

        // POST: NotaCredito/CrearNotaCredito
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CrearNotaCredito(NotaCredito nota)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.NotaCredito.Add(nota);
                    _context.SaveChanges();
                    TempData["SuccessMessage"] = "Nota de crédito creada exitosamente.";
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Ocurrió un error al guardar la nota de crédito: " + ex.Message);
                }
            }

            // Recargar datos de la factura si hay error
            var factura = _context.Facturas.FirstOrDefault(f => f.FacturaID == nota.FacturaID);
            if (factura != null)
            {
                ViewBag.FacturaNumero = factura.FacturaID;
                ViewBag.FacturaCliente = factura.ClienteNombre;
                ViewBag.FacturaTotal = factura.Total;
            }

            return View("Crear", nota);
        }

        // GET: NotaCredito/Detalle/{id}
        public IActionResult Detalle(int id)
        {
            var nota = _context.NotaCredito.FirstOrDefault(n => n.NotaCreditoID == id);
            if (nota == null)
            {
                return NotFound();
            }

            // Cargar información relacionada si es necesario
            ViewBag.Factura = _context.Facturas.FirstOrDefault(f => f.FacturaID == nota.FacturaID);

            return View(nota);
        }

        // GET: NotaCredito/Imprimir/{id}
        public IActionResult Imprimir(int id)
        {
            var nota = _context.NotaCredito.FirstOrDefault(n => n.NotaCreditoID == id);
            if (nota == null)
            {
                return NotFound();
            }

            // Configurar vista para impresión
            ViewBag.Factura = _context.Facturas.FirstOrDefault(f => f.FacturaID == nota.FacturaID);
            return View("Detalle", nota); // Reutilizar vista de detalle o crear una específica para impresión
        }
    }
}