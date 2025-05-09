using ClosedXML.Excel;
using ProyectoFinal_PrograIII.Data;
using ProyectoFinal_PrograIII.Modelo;
using Microsoft.EntityFrameworkCore;
using ProyectoFinal_PrograIII.ApiECommerce.IServices;

namespace ProyectoFinal_PrograIII.Servicio
{
    public class InventarioServicio : IInventarioService
    {
        private readonly ApplicationDbContext _context;

        public InventarioServicio(ApplicationDbContext context)
        {
            _context = context;
        }

        
            public async Task<bool> ActualizarInventarioPorCompra(DetalleCompra detalleCompra)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var producto = await _context.productos.FindAsync(detalleCompra.IdProductos);
                if (producto == null) return false;

                producto.Existencias += detalleCompra.CantidadProductos;

                var movimiento = new MovimientoInventario
                {
                    Fecha = DateTime.Now,
                    IdProductos = detalleCompra.IdProductos,
                    Cantidad = detalleCompra.CantidadProductos,
                    TipoMovimiento = "Entrada",
                    Referencia = $"Compra #{detalleCompra.IdCompras}",
                    IdCompras = detalleCompra.IdCompras,
                    PrecioUnitario = detalleCompra.PrecioUnitario,
                    SubTotal = detalleCompra.SubTotal
                };

                _context.MovimientosInventario.Add(movimiento);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                return true;
            }
            catch
            {  await transaction.RollbackAsync();
                return false;
            }
        }

        public async Task<bool> ActualizarInventarioPorVenta(DetallePedido detallePedido)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var producto = await _context.productos.FindAsync(detallePedido.IdProductos);
                if (producto == null || producto.Existencias < detallePedido.CantidadProductos)
                    return false;

                producto.Existencias -= detallePedido.CantidadProductos;

                var movimiento = new MovimientoInventario
                {
                    Fecha = DateTime.Now,
                    IdProductos = detallePedido.IdProductos,
                    Cantidad = detallePedido.CantidadProductos,
                    TipoMovimiento = "Salida",
                    Referencia = $"Venta #{detallePedido.IdPedidos}",
                    IdPedidos = detallePedido.IdPedidos,
                    PrecioUnitario = detallePedido.PrecioUnitario,
                    SubTotal = detallePedido.SubTotal
                };

                _context.MovimientosInventario.Add(movimiento);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                return true;
            }
            catch (Exception ex)
            { 
                await transaction.RollbackAsync();
                return false;
            }
        }
        public async Task<byte[]> GenerarExcelCompras(DateTime? fechaInicio = null, DateTime? fechaFin = null)
        {
            try
            {
                var compras = await _context.compras
                    .Include(c => c.Proveedor)
                    .Include(c => c.DetalleCompras)
                        .ThenInclude(d => d.Producto)
                    .Where(c => c.Estado != "Cancelado" && 
                            c.DetalleCompras.Any()&&
                            (!fechaInicio.HasValue || c.Fecha >= fechaInicio.Value) &&
                            (!fechaFin.HasValue || c.Fecha <= fechaFin.Value))
                    .OrderByDescending(c => c.Fecha)
                    .ToListAsync();

                if (!compras.Any())
                {
                    throw new Exception("No se encontraron registros de compras");
                }

                using var workbook = new XLWorkbook();
                var worksheet = workbook.Worksheets.Add("Compras");

                // Encabezados
                worksheet.Cell(1, 1).Value = "Fecha";
                worksheet.Cell(1, 2).Value = "N° Compra";
                worksheet.Cell(1, 3).Value = "Proveedor";
                worksheet.Cell(1, 4).Value = "Producto";
                worksheet.Cell(1, 5).Value = "Cantidad";
                worksheet.Cell(1, 6).Value = "Precio Unitario";
                worksheet.Cell(1, 7).Value = "SubTotal";
                worksheet.Cell(1, 8).Value = "Estado";

                // Formato encabezados
                var headerRange = worksheet.Range(1, 1, 1, 8);
                headerRange.Style.Fill.BackgroundColor = XLColor.LightGray;
                headerRange.Style.Font.Bold = true;

                var row = 2;
                decimal totalCompras = 0;

                foreach (var compra in compras)
                {
                    if (compra.DetalleCompras != null)
                    {
                        foreach (var detalle in compra.DetalleCompras)
                        {
                            worksheet.Cell(row, 1).Value = compra.Fecha.ToString("dd/MM/yyyy HH:mm");
                            worksheet.Cell(row, 2).Value = compra.Id;
                            worksheet.Cell(row, 3).Value = compra.Proveedor?.Nombre ?? "Proveedor no especificado";
                            worksheet.Cell(row, 4).Value = detalle.Producto?.Nombre ?? "Producto no especificado";
                            worksheet.Cell(row, 5).Value = detalle.CantidadProductos;
                            worksheet.Cell(row, 6).Value = detalle.PrecioUnitario;
                            worksheet.Cell(row, 7).Value = detalle.SubTotal;
                            worksheet.Cell(row, 8).Value = compra.Estado ?? "No especificado";
                            worksheet.Cell(row, 9).Style.NumberFormat.Format = "#,##0.00";
                            worksheet.Cell(row, 10).Style.NumberFormat.Format = "#,##0.00";

                            totalCompras += detalle.SubTotal;
                            row++;
                        }
                    }
                }

                // Formato tabla
                var tableRange = worksheet.Range(1, 1, row - 1, 8);
                tableRange.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                tableRange.Style.Border.InsideBorder = XLBorderStyleValues.Thin;

                // Ajustar columnas
                worksheet.Columns().AdjustToContents();

                // Agregar totales
                row++;
                worksheet.Cell(row, 6).Value = "Total Compras:";
                worksheet.Cell(row, 7).Value = totalCompras;
                var totalRange = worksheet.Range(row, 6, row, 7);
                totalRange.Style.Font.Bold = true;
                totalRange.Style.Fill.BackgroundColor = XLColor.LightGray;

                using var stream = new MemoryStream();
                workbook.SaveAs(stream);
                return stream.ToArray();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al generar Excel de compras: {ex.Message}");
            }

        }
        public async Task<byte[]> GenerarExcelCompras()
        {
            return await GenerarExcelCompras(null, null);
        }

        public async Task<byte[]> GenerarExcelVentas()
        {
            return await GenerarExcelVentas(null, null);
        }
        public async Task<byte[]> GenerarExcelVentas(DateTime? fechaInicio = null, DateTime? fechaFin = null)
        {
            try
            {
                var ventas = await _context.pedidos
                    .Include(p => p.Cliente)
                    .Include(p => p.DetallesPedido)
                        .ThenInclude(d => d.Producto)
                    .Where(p => p.Estado != "Cancelado"&&
                            p.DetallesPedido.Any()&&
                            (!fechaInicio.HasValue || p.Fecha >= fechaInicio.Value) &&
                            (!fechaFin.HasValue || p.Fecha <= fechaFin.Value))
                    .OrderByDescending(p => p.Fecha)
                    .ToListAsync();

                if (!ventas.Any())
                {
                    throw new Exception("No se encontraron registros de ventas");
                }

                using var workbook = new XLWorkbook();
                var worksheet = workbook.Worksheets.Add("Ventas");

                // Encabezados
                worksheet.Cell(1, 1).Value = "Fecha";
                worksheet.Cell(1, 2).Value = "N° Pedido";
                worksheet.Cell(1, 3).Value = "Cliente";
                worksheet.Cell(1, 4).Value = "Producto";
                worksheet.Cell(1, 5).Value = "Cantidad";
                worksheet.Cell(1, 6).Value = "Precio Unitario";
                worksheet.Cell(1, 7).Value = "SubTotal";
                worksheet.Cell(1, 8).Value = "Estado";

                // Formato encabezados
                var headerRange = worksheet.Range(1, 1, 1, 8);
                headerRange.Style.Fill.BackgroundColor = XLColor.LightGray;
                headerRange.Style.Font.Bold = true;

                var row = 2;
                decimal totalVentas = 0;

                foreach (var venta in ventas)
                {
                    foreach (var detalle in venta.DetallesPedido)
                    {
                        worksheet.Cell(row, 1).Value = venta.Fecha.ToString("dd/MM/yyyy HH:mm");
                        worksheet.Cell(row, 2).Value = venta.Id;
                        worksheet.Cell(row, 3).Value = venta.Cliente?.Nombre ?? "Cliente no especificado";
                        worksheet.Cell(row, 4).Value = detalle.Producto?.Nombre ?? "Producto no especificado";
                        worksheet.Cell(row, 5).Value = detalle.CantidadProductos;
                        worksheet.Cell(row, 6).Value = detalle.PrecioUnitario;
                        worksheet.Cell(row, 7).Value = detalle.SubTotal;
                        worksheet.Cell(row, 8).Value = venta.Estado ?? "No especificado";
                        worksheet.Cell(row, 9).Style.NumberFormat.Format = "#,##0.00";
                        worksheet.Cell(row, 10).Style.NumberFormat.Format = "#,##0.00";

                        totalVentas += detalle.SubTotal;
                        row++;
                    }
                }

                // Formato tabla
                var tableRange = worksheet.Range(1, 1, row - 1, 8);
                tableRange.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                tableRange.Style.Border.InsideBorder = XLBorderStyleValues.Thin;

                // Ajustar columnas
                worksheet.Columns().AdjustToContents();

                // Agregar totales
                row++;
                worksheet.Cell(row, 6).Value = "Total Ventas:";
                worksheet.Cell(row, 7).Value = totalVentas;
                var totalRange = worksheet.Range(row, 6, row, 7);
                totalRange.Style.Font.Bold = true;
                totalRange.Style.Fill.BackgroundColor = XLColor.LightGray;

                using var stream = new MemoryStream();
                workbook.SaveAs(stream);
                return stream.ToArray();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al generar Excel de ventas: {ex.Message}");
            }
        }

        public async Task<bool> ValidarExistencias(int productoId, int cantidad)
        {
            var producto = await _context.productos.FindAsync(productoId);
            return producto != null && producto.Existencias >= cantidad;
        }

        public async Task<decimal> ObtenerValorInventarioTotal()
        {
            return await _context.productos.SumAsync(p => p.Existencias * p.Precio);
        }

        public async Task<IEnumerable<MovimientoInventario>> ObtenerMovimientosPorProducto(int productoId)
        {
            return await _context.MovimientosInventario
                .Include(m => m.Producto)
                .Where(m => m.IdProductos == productoId)
                .OrderByDescending(m => m.Fecha)
                .ToListAsync();
        }
        public async Task<Dictionary<string, decimal>> ObtenerResumenInventario()
        {
            var totalCompras = await _context.compras
                .Where(c => c.Estado != "Cancelado")
                .SelectMany(c => c.DetalleCompras)
                .SumAsync(d => d.SubTotal);

            var totalVentas = await _context.pedidos
                .Where(p => p.Estado != "Cancelado")
                .SelectMany(p => p.DetallesPedido)
                .SumAsync(d => d.SubTotal);

            var valorInventario = await ObtenerValorInventarioTotal();

            return new Dictionary<string, decimal>
            {
                { "TotalCompras", totalCompras },
                { "TotalVentas", totalVentas },
                { "ValorInventario", valorInventario }
            };
        }
    }

}
