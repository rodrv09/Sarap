
    using ClosedXML.Excel;
    using global::Sarap.Models;
    using Microsoft.AspNetCore.Mvc;
    using QuestPDF.Fluent;
    using QuestPDF.Infrastructure;
    using System;
    using System.IO;
    using System.Linq;

    namespace Sarap.Controllers
    {
        public class ReportesController : Controller
        {
            private readonly EspeciasSarapiquiContext _context;

            public ReportesController(EspeciasSarapiquiContext context)
            {
                _context = context;
            }
        public IActionResult Index()
        {
            return View();
        }


        // Excel export - genérico para cada tabla
        private FileContentResult ExportToExcel<T>(IQueryable<T> data, string sheetName, Action<IXLWorksheet> fillSheet)
            {
                using var workbook = new XLWorkbook();
                var worksheet = workbook.Worksheets.Add(sheetName);
                fillSheet(worksheet);

                using var stream = new MemoryStream();
                workbook.SaveAs(stream);
                var content = stream.ToArray();
                string fileName = $"Reporte_{sheetName}_{DateTime.Now:yyyyMMddHHmmss}.xlsx";
                return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
            }

        // Clientes
        public IActionResult ExportarExcelClientes()
        {
            var clientes = _context.Clientes.OrderBy(c => c.ClienteId).AsQueryable();
            return ExportToExcel(clientes, "Clientes", sheet =>
            {
                sheet.Cell(1, 1).Value = "ID";
                sheet.Cell(1, 2).Value = "Nombre";
                sheet.Cell(1, 3).Value = "Apellido";
                sheet.Cell(1, 4).Value = "Teléfono";
                sheet.Cell(1, 5).Value = "Email";
                sheet.Cell(1, 6).Value = "Fecha Registro";
                sheet.Cell(1, 7).Value = "Activo";

                int row = 2;
                foreach (var c in clientes)
                {
                    sheet.Cell(row, 1).Value = c.ClienteId;
                    sheet.Cell(row, 2).Value = c.Nombre;
                    sheet.Cell(row, 3).Value = c.Apellido;
                    sheet.Cell(row, 4).Value = c.Telefono;
                    sheet.Cell(row, 5).Value = c.Email;

                    // Aquí la corrección para fecha nullable
                    sheet.Cell(row, 6).Value = c.FechaRegistro?.ToShortDateString() ?? "";

                    sheet.Cell(row, 7).Value = c.Activo ? "Sí" : "No";
                    row++;
                }

                sheet.Columns().AdjustToContents();
            });
        }



        // Proveedores
        public IActionResult ExportarExcelProveedores()
            {
                var proveedores = _context.Proveedores.OrderBy(p => p.ProveedorId).AsQueryable();
                return ExportToExcel(proveedores, "Proveedores", sheet =>
                {
                    sheet.Cell(1, 1).Value = "ID";
                    sheet.Cell(1, 2).Value = "Nombre Empresa";
                    sheet.Cell(1, 3).Value = "Contacto";
                    sheet.Cell(1, 4).Value = "Teléfono";
                    sheet.Cell(1, 5).Value = "Email";
                    sheet.Cell(1, 6).Value = "Fecha Registro";
                    sheet.Cell(1, 7).Value = "Activo";

                    int row = 2;
                    foreach (var p in proveedores)
                    {
                        sheet.Cell(row, 1).Value = p.ProveedorId;
                        sheet.Cell(row, 2).Value = p.NombreEmpresa;
                        sheet.Cell(row, 3).Value = p.ContactoNombre;
                        sheet.Cell(row, 4).Value = p.Telefono;
                        sheet.Cell(row, 5).Value = p.Email;
                        sheet.Cell(row, 6).Value = p.FechaRegistro?.ToShortDateString() ?? "";
                        sheet.Cell(row, 7).Value = p.Activo ? "Sí" : "No";
                        row++;
                    }
                    sheet.Columns().AdjustToContents();
                });
            }

          

            // Usuarios
            public IActionResult ExportarExcelUsuarios()
            {
                var usuarios = _context.Usuarios.OrderBy(u => u.UsuarioId).AsQueryable();
                return ExportToExcel(usuarios, "Usuarios", sheet =>
                {
                    sheet.Cell(1, 1).Value = "ID";
                    sheet.Cell(1, 2).Value = "Nombre Usuario";
                    sheet.Cell(1, 3).Value = "Nombre";
                    sheet.Cell(1, 4).Value = "Apellido";
                    sheet.Cell(1, 5).Value = "Email";
                    sheet.Cell(1, 6).Value = "Rol";
                    sheet.Cell(1, 7).Value = "Activo";

                    int row = 2;
                    foreach (var u in usuarios)
                    {
                        sheet.Cell(row, 1).Value = u.UsuarioId;
                        sheet.Cell(row, 2).Value = u.NombreUsuario;
                        sheet.Cell(row, 3).Value = u.Nombre;
                        sheet.Cell(row, 4).Value = u.Apellido;
                        sheet.Cell(row, 5).Value = u.Email;
                        sheet.Cell(row, 6).Value = u.Rol;
                        sheet.Cell(row, 7).Value = u.Activo ? "Sí" : "No";
                        row++;
                    }
                    sheet.Columns().AdjustToContents();
                });
            }

           

            // Agrega aquí métodos similares para las demás tablas que quieras exportar,
            // por ejemplo, Productos, Empleados, Facturas, etc.

            // Ejemplo para Facturas (solo listado básico)
            public IActionResult ExportarExcelFacturas()
            {
                var facturas = _context.Facturas.OrderBy(f => f.FacturaID).AsQueryable();
                return ExportToExcel(facturas, "Facturas", sheet =>
                {
                    sheet.Cell(1, 1).Value = "ID";
                    sheet.Cell(1, 2).Value = "Fecha";
                    sheet.Cell(1, 3).Value = "Cliente";
                    sheet.Cell(1, 4).Value = "Total";
                    sheet.Cell(1, 5).Value = "Tipo de Pago";
                    sheet.Cell(1, 6).Value = "Usuario";

                    int row = 2;
                    foreach (var f in facturas)
                    {
                        sheet.Cell(row, 1).Value = f.FacturaID;
                        sheet.Cell(row, 2).Value = f.Fecha.ToShortDateString();
                        sheet.Cell(row, 3).Value = f.ClienteNombre;
                        sheet.Cell(row, 4).Value = f.Total;
                        sheet.Cell(row, 5).Value = f.TipoPago;
                        sheet.Cell(row, 6).Value = f.Usuario;
                        row++;
                    }
                    sheet.Columns().AdjustToContents();
                });
            }

         
        }
    }


