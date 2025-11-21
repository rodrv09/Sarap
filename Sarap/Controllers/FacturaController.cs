using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using Sarap.Models;
using SelectPdf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SelectPdf;
using System.Text;


namespace Sarap.Controllers
{
    public class FacturasController : Controller
    {
        private readonly EspeciasSarapiquiContext _context;

        public FacturasController(EspeciasSarapiquiContext context)
        {
            _context = context;
        }

        // GET: Facturas
        public IActionResult Index(int pagina = 1)
        {
            int tamanoPagina = 10;

            var facturas = _context.Facturas
                .OrderByDescending(f => f.Fecha)
                .ToList();

            int totalFacturas = facturas.Count;
            int totalPaginas = (int)Math.Ceiling(totalFacturas / (double)tamanoPagina);

            var facturasPagina = facturas
                .Skip((pagina - 1) * tamanoPagina)
                .Take(tamanoPagina)
                .ToList();

            var viewModel = new FacturaPaginadaViewModel
            {
                Facturas = facturasPagina,
                PaginaActual = pagina,
                TotalPaginas = totalPaginas
            };

            return View(viewModel);
        }


        // GET: Facturas/Crear
        public IActionResult Crear()
        {
            var model = new FacturaViewModel
            {
                Factura = new Factura { Fecha = DateTime.Now },
                NuevoDetalle = new FacturaDetalle(),
                FacturaDetalles = new List<FacturaDetalle>(),
                ProductosSelectList = ObtenerProductosSelectList(),
                ClientesSelectList = ObtenerClientesSelectList()
            };

            return View(model);
        }

        // POST: Facturas/CrearFactura
        [HttpPost]
        public async Task<IActionResult> CrearFactura(FacturaViewModel model, string accion)
        {
            model.ProductosSelectList = ObtenerProductosSelectList();
            model.ClientesSelectList = ObtenerClientesSelectList();

            var detalles = TempData["FacturaDetalles"] != null
                ? JsonConvert.DeserializeObject<List<FacturaDetalle>>(TempData["FacturaDetalles"].ToString())
                : new List<FacturaDetalle>();

            if (accion == "AgregarProducto")
            {
                var producto = _context.Productos.FirstOrDefault(p => p.ProductoID == model.NuevoDetalle.ProductoID);
                if (producto != null)
                {
                    var detalle = new FacturaDetalle
                    {
                        ProductoID = producto.ProductoID,
                        NombreProducto = producto.Nombre,
                        PrecioUnitario = producto.Precio,
                        Cantidad = model.NuevoDetalle.Cantidad,
                        Subtotal = producto.Precio * model.NuevoDetalle.Cantidad,
                        TotalLinea = producto.Precio * model.NuevoDetalle.Cantidad
                    };

                    detalles.Add(detalle);
                }

                TempData["FacturaDetalles"] = JsonConvert.SerializeObject(detalles);

                model.FacturaDetalles = detalles;
                model.NuevoDetalle = new FacturaDetalle();

                return View("Crear", model);
            }
            else if (accion == "FinalizarFactura")
            {
                model.FacturaDetalles = detalles;

                if (!model.FacturaDetalles.Any())
                {
                    ModelState.AddModelError("", "Debe agregar al menos un producto.");
                    return View("Crear", model);
                }

                // Validar ClienteIdentidad (string) y convertir a int para búsqueda
                if (string.IsNullOrEmpty(model.Factura.ClienteIdentidad) ||
                    !int.TryParse(model.Factura.ClienteIdentidad, out int clienteId) ||
                    clienteId <= 0)
                {
                    ModelState.AddModelError("Factura.ClienteIdentidad", "Debe seleccionar un cliente válido.");
                    return View("Crear", model);
                }

                var cliente = _context.Clientes.FirstOrDefault(c => c.ClienteId == clienteId);
                if (cliente == null)
                {
                    ModelState.AddModelError("Factura.ClienteIdentidad", "Cliente seleccionado no existe.");
                    return View("Crear", model);
                }

                model.Factura.ClienteNombre = cliente.Nombre + " " + cliente.Apellido;

                model.Factura.Total = model.FacturaDetalles.Sum(d => d.TotalLinea);
                model.Factura.Subtotal = model.FacturaDetalles.Sum(d => d.Subtotal);
                model.Factura.Impuesto = model.FacturaDetalles.Sum(d => d.Impuesto ?? 0);
                model.Factura.Descuento = model.FacturaDetalles.Sum(d => d.Descuento ?? 0);

                _context.Facturas.Add(model.Factura);
                await _context.SaveChangesAsync();

                foreach (var detalle in model.FacturaDetalles)
                {
                    detalle.FacturaID = model.Factura.FacturaID;
                    _context.FacturaDetalles.Add(detalle);
                }

                await _context.SaveChangesAsync();

                TempData.Remove("FacturaDetalles");
                TempData["Mensaje"] = "Factura creada correctamente.";

                return RedirectToAction(nameof(Index));
            }

            return View("Crear", model);
        }

        private List<SelectListItem> ObtenerProductosSelectList()
        {
            return _context.Productos.Select(p => new SelectListItem
            {
                Value = p.ProductoID.ToString(),
                Text = p.Nombre
            }).ToList();
        }

        private List<SelectListItem> ObtenerClientesSelectList()
        {
            return _context.Clientes
                .Where(c => c.Activo)
                .Select(c => new SelectListItem
                {
                    Value = c.ClienteId.ToString(),
                    Text = c.Nombre + " " + c.Apellido
                }).ToList();
        }
        // GET: Facturas/CrearNotaCredito/5
        public IActionResult CrearNotaCredito(int facturaId)
        {
            var factura = _context.Facturas.FirstOrDefault(f => f.FacturaID == facturaId);
            if (factura == null)
            {
                return NotFound();
            }

            var model = new NotaCredito
            {
                FacturaID = facturaId,
                Fecha = DateTime.Now
            };

            ViewBag.FacturaNumero = factura.FacturaID;
            ViewBag.FacturaCliente = factura.ClienteNombre;
            ViewBag.FacturaTotal = factura.Total;

            return View(model);
        }

        // POST: Facturas/CrearNotaCredito
        [HttpPost]
        public async Task<IActionResult> CrearNotaCredito(NotaCredito model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var factura = _context.Facturas.FirstOrDefault(f => f.FacturaID == model.FacturaID);
            if (factura == null)
            {
                return NotFound();
            }

            if (model.Monto <= 0 || model.Monto > factura.Total)
            {
                ModelState.AddModelError("Monto", "El monto debe ser mayor que cero y menor o igual al total de la factura.");
                return View(model);
            }

            _context.NotaCredito.Add(model);
            await _context.SaveChangesAsync();

            // Aquí puedes poner lógica adicional, como actualizar el saldo de la factura, si quieres

            TempData["Mensaje"] = "Nota de crédito creada correctamente.";

            return RedirectToAction(nameof(Index));
        }
        public IActionResult DetalleFactura(int facturaId)
        {
            var factura = _context.Facturas
                .Where(f => f.FacturaID == facturaId)
                .FirstOrDefault();

            if (factura == null)
            {
                return NotFound();
            }

            // Aquí puedes cargar también los detalles de la factura si quieres
            var detalles = _context.FacturaDetalles
                .Where(d => d.FacturaID == facturaId)
                .ToList();

            var viewModel = new FacturaDetalleViewModel
            {
                Factura = factura,
                Detalles = detalles
            };

            return View(viewModel);
        }

        private string GenerarHtmlFactura(Factura factura, List<FacturaDetalle> detalles, string baseUrl)
        {
            var sb = new StringBuilder();

            sb.Append($@"
<html>
<head>
    <style>
        body {{
            font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
            color: #333;
            margin: 0;
            padding: 0;
            background-color: #f4f6f9;
        }}
        .factura-container {{
            max-width: 1100px;
            margin: auto;
            padding: 2rem;
        }}

        .header {{
            background-color: #1f2d3d; /* Mismo azul oscuro del layout */
            color: white;
            display: flex;
            align-items: center;
            padding: 1rem 2rem;
            gap: 1.5rem;
            margin-bottom: 2rem;
            border-bottom: 4px solid #1abc9c;
        }}
        .header img {{
            width: 60px;
            height: auto;
        }}
        .header .info {{
            font-size: 0.95rem;
            line-height: 1.4;
        }}
        .header .info strong {{
            font-size: 1.2rem;
            display: block;
        }}

        h2 {{
            text-align: center;
            margin-top: 0;
            margin-bottom: 1rem;
            color: #1f2d3d;
        }}

        table {{
            width: 100%;
            border-collapse: collapse;
            margin-top: 1rem;
        }}
        th, td {{
            border: 1px solid #ccc;
            padding: 8px;
            text-align: center;
        }}
        th {{
            background-color: #e9ecef;
        }}

        .total {{
            text-align: right;
            font-weight: bold;
            margin-top: 1rem;
            font-size: 1.1rem;
            color: #1f2d3d;
        }}
    </style>
</head>
<body>
    <div class='factura-container'>
        <header class='header'>
            <img src='{baseUrl}/images/logo.png' alt='Logo Especias Sarapiquí' />
            <div class='info'>
                <strong>Especias Sarapiquí</strong>
                Teléfono: 2441 5050<br/>
                San Juan Sur, Poás, Alajuela
            </div>
        </header>

        <h2>Factura No. {factura.FacturaID}</h2>
        <p><strong>Cliente:</strong> {factura.ClienteNombre}</p>
        <p><strong>Fecha:</strong> {factura.Fecha.ToShortDateString()}</p>

        <table>
            <thead>
                <tr>
                    <th>Producto</th>
                    <th>Cantidad</th>
                    <th>Precio Unitario</th>
                    <th>Subtotal</th>
                    <th>Impuesto</th>
                    <th>Descuento</th>
                    <th>Total Línea</th>
                </tr>
            </thead>
            <tbody>");

            foreach (var d in detalles)
            {
                sb.Append($@"
                <tr>
                    <td>{d.NombreProducto}</td>
                    <td>{d.Cantidad}</td>
                    <td>{d.PrecioUnitario:C}</td>
                    <td>{d.Subtotal:C}</td>
                    <td>{(d.Impuesto.HasValue ? d.Impuesto.Value.ToString("P2") : "-")}</td>
                    <td>{(d.Descuento.HasValue ? d.Descuento.Value.ToString("P2") : "-")}</td>
                    <td>{d.TotalLinea:C}</td>
                </tr>");
            }

            sb.Append($@"
            </tbody>
        </table>

        <p class='total'>Total: {factura.Total:C}</p>
    </div>
</body>
</html>");

            return sb.ToString();
        }

        public IActionResult ExportarPdf(int facturaId)
        {
            var factura = _context.Facturas.FirstOrDefault(f => f.FacturaID == facturaId);
            if (factura == null)
                return NotFound();

            var detalles = _context.FacturaDetalles.Where(d => d.FacturaID == facturaId).ToList();

            // Obtener URL base (ejemplo para localhost)
            var baseUrl = $"{Request.Scheme}://{Request.Host}";

            var htmlContent = GenerarHtmlFactura(factura, detalles, baseUrl);

            var pdf = new HtmlToPdf();
            var doc = pdf.ConvertHtmlString(htmlContent);

            return File(doc.Save(), "application/pdf", $"Factura_{factura.FacturaID}.pdf");
        }




    }

}
