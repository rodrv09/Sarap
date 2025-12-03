using Microsoft.AspNetCore.Mvc;
using Sarap.Models;
using System;
using System.Linq;

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
        // Si alguien entra directo a /NotaCredito/Crear, lo mando a la lista de facturas
        public IActionResult Crear()
        {
            return RedirectToAction("Index", "Facturas");
        }

        // GET: NotaCredito/CrearParaFactura/{facturaId}
        public IActionResult CrearParaFactura(int facturaId)
        {
            var factura = _context.Facturas.FirstOrDefault(f => f.FacturaID == facturaId);
            if (factura == null)
            {
                return NotFound();
            }

            // Total de notas de crédito ya aplicadas a esta factura
            var totalNotasPrevias = _context.NotaCredito
                .Where(n => n.FacturaID == factura.FacturaID)
                .Sum(n => n.Monto);

            var saldoDisponible = factura.Total - totalNotasPrevias;

            ViewBag.FacturaNumero = factura.FacturaID;
            ViewBag.FacturaCliente = factura.ClienteNombre;
            ViewBag.FacturaTotal = factura.Total;
            ViewBag.SaldoDisponible = saldoDisponible;

            var nota = new NotaCredito
            {
                FacturaID = factura.FacturaID,
                Fecha = DateTime.Now // Fecha actual por defecto
            };

            // Usa la vista ubicada en Views/Facturas/CrearNotaCredito.cshtml
            return View("~/Views/Facturas/CrearNotaCredito.cshtml", nota);
        }

        // POST: NotaCredito/CrearNotaCredito
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CrearNotaCredito(NotaCredito nota)
        {
            // Buscar la factura asociada
            var factura = _context.Facturas.FirstOrDefault(f => f.FacturaID == nota.FacturaID);
            if (factura == null)
            {
                ModelState.AddModelError("", "La factura asociada a la nota de crédito no existe.");
            }
            else
            {
                // Total de notas ya registradas para esta factura
                var totalNotasPrevias = _context.NotaCredito
                    .Where(n => n.FacturaID == nota.FacturaID)
                    .Sum(n => n.Monto);

                var saldoDisponible = factura.Total - totalNotasPrevias;

                // Guardamos el saldo en ViewBag para mostrarlo en la vista
                ViewBag.SaldoDisponible = saldoDisponible;

                // Validaciones de negocio
                if (nota.Monto <= 0)
                {
                    ModelState.AddModelError("Monto", "El monto debe ser mayor que cero.");
                }
                else if (nota.Monto > saldoDisponible)
                {
                    ModelState.AddModelError(
                        "Monto",
                        $"El monto excede el saldo disponible de la factura. " +
                        $"Saldo máximo permitido: {saldoDisponible:C} (considerando notas de crédito anteriores)."
                    );
                }

                // Datos para mostrar en la vista
                ViewBag.FacturaNumero = factura.FacturaID;
                ViewBag.FacturaCliente = factura.ClienteNombre;
                ViewBag.FacturaTotal = factura.Total;
            }

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

            // Si hubo error, volvemos a la misma vista de CrearNotaCredito en /Views/Facturas
            return View("~/Views/Facturas/CrearNotaCredito.cshtml", nota);
        }

        // GET: NotaCredito/Detalle/{id}
        public IActionResult Detalle(int id)
        {
            var nota = _context.NotaCredito.FirstOrDefault(n => n.NotaCreditoID == id);
            if (nota == null)
            {
                return NotFound();
            }

            ViewBag.Factura = _context.Facturas.FirstOrDefault(f => f.FacturaID == nota.FacturaID);

            // Usa la vista Views/NotaCredito/Detalle.cshtml por convención
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

            ViewBag.Factura = _context.Facturas.FirstOrDefault(f => f.FacturaID == nota.FacturaID);

            // Reutiliza la misma vista que Detalle
            return View("Detalle", nota);
        }
        // GET: NotaCredito/Eliminar/{id}
        public IActionResult Eliminar(int id)
        {
            var nota = _context.NotaCredito.FirstOrDefault(n => n.NotaCreditoID == id);
            if (nota == null)
            {
                return NotFound();
            }

            ViewBag.Factura = _context.Facturas.FirstOrDefault(f => f.FacturaID == nota.FacturaID);

            return View(nota);
        }
// POST: NotaCredito/EliminarConfirmado/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EliminarConfirmado(int id)
        {
            var nota = _context.NotaCredito.FirstOrDefault(n => n.NotaCreditoID == id);

            if (nota == null)
            {
                TempData["ErrorMessage"] = "La nota que intenta eliminar no existe.";
                return RedirectToAction("Index");
            }

            try
            {
                _context.NotaCredito.Remove(nota);
                _context.SaveChanges();
                TempData["SuccessMessage"] = "Nota de crédito eliminada correctamente.";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error eliminando la nota: " + ex.Message;
            }

            return RedirectToAction("Index");
        }

    }
}
