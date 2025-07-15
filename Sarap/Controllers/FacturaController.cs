using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Sarap.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
        public IActionResult Index()
        {
            var facturas = _context.Facturas.ToList();

            var viewModel = new FacturaViewModel
            {
                Facturas = facturas
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
                ProductosSelectList = ObtenerProductosSelectList()
            };

            return View(model);
        }

        // POST: Facturas/CrearFactura
        [HttpPost]
        public async Task<IActionResult> CrearFactura(FacturaViewModel model, string accion)
        {
            model.ProductosSelectList = ObtenerProductosSelectList();

            // Leer los detalles anteriores desde TempData
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
                        DetalleID = new Random().Next(1000, 9999),
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


        // Método auxiliar para el dropdown
        private List<SelectListItem> ObtenerProductosSelectList()
        {
            return _context.Productos.Select(p => new SelectListItem
            {
                Value = p.ProductoID.ToString(),
                Text = p.Nombre
            }).ToList();
        }
    }
}

