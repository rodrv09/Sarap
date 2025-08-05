using Microsoft.AspNetCore.Mvc;
using Sarap.Models;

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

        // GET: NotaCredito/SeleccionarFactura
        public IActionResult SeleccionarFactura()
        {
            var facturas = _context.Facturas
                .Where(f => f.Fecha >= DateTime.Now.AddDays(-30))
                .OrderByDescending(f => f.Fecha)
                .ToList();

            return View(facturas);
        }

        // GET: NotaCredito/Crear?facturaId=5
        // GET: /NotaCredito/Crear?facturaId=123
        [HttpGet]
        public IActionResult Crear(int facturaId)
        {
            // 1. Validar que la factura exista
            var factura = _context.Facturas
                .FirstOrDefault(f => f.FacturaID == facturaId);

            if (factura == null)
            {
                TempData["ErrorMessage"] = "La factura seleccionada no existe";
                return RedirectToAction("SeleccionarFactura");
            }

            // 2. Crear el modelo inicial
            var modelo = new NotaCredito
            {
                FacturaID = facturaId,
                Fecha = DateTime.Now,
                Monto = factura.Total // Puedes poner el total o dejar 0 para que lo modifiquen
            };

            // 3. Pasar datos adicionales a la vista
            ViewBag.Factura = factura; // Para mostrar detalles de la factura

            return View(modelo); // Renderiza Views/NotaCredito/Crear.cshtml
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

                    TempData["SuccessMessage"] = $"Nota de crédito #{nota.NotaCreditoID} creada correctamente";
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"Error al crear la nota de crédito: {ex.Message}");
                }
            }

            var factura = _context.Facturas.FirstOrDefault(f => f.FacturaID == nota.FacturaID);
            if (factura != null)
            {
                ViewBag.FacturaNumero = factura.FacturaID;
                ViewBag.FacturaCliente = factura.ClienteNombre;
                ViewBag.FacturaTotal = factura.Total;
                ViewBag.FacturaInfo = factura;
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

            ViewBag.Factura = _context.Facturas.FirstOrDefault(f => f.FacturaID == nota.FacturaID);
            return View("Detalle", nota); // o una vista personalizada para impresión
        }
    }
}
