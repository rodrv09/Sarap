using Microsoft.AspNetCore.Mvc;
using Sarap.Models;
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
            var notas = _context.NotaCredito.ToList();
            return View(notas);
        }

        // GET: NotaCredito/Crear/{facturaId}
        public IActionResult Crear(int facturaId)
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
                FacturaID = factura.FacturaID
            };

            return View(nota);
        }

        // POST: NotaCredito/CrearNotaCredito
        [HttpPost]
        public IActionResult CrearNotaCredito(NotaCredito nota)
        {
            if (ModelState.IsValid)
            {
                _context.NotaCredito.Add(nota);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }

            return View("Crear", nota);
        }
    }
}
