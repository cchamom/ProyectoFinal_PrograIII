using ProyectoFinal_PrograIII.Modelo;
using ProyectoFinal_PrograIII.Data;
using Microsoft.EntityFrameworkCore;
using ProyectoFinal_PrograIII.ApiECommerce.IServices;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProyectoFinal_PrograIII.Servicio
{
    public class VentaServicio : IVentasService
    {
        private readonly ApplicationDbContext _context;

        public VentaServicio(ApplicationDbContext context)
        {
            _context = context;
        }
        // Constructor del servicio de inventario
        // Esto es útil para actualizar el inventario después de una venta
        private readonly IInventarioService _inventarioService;
        public VentaServicio(ApplicationDbContext context, IInventarioService inventarioService)
        {
            _context = context;
            _inventarioService = inventarioService;
        }
        public async Task<IEnumerable<Ventas>> ObtenerVentasAsync()
        {
            try
            {
                return await _context.Venta
                    .Include(v => v.Cliente)
                    .Include(v => v.DetallesPedidos)
                        .ThenInclude(dp => dp.Producto)
                    .AsNoTracking()
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener las ventas ", ex);
            }
        }

        public async Task<Ventas> ObtenerVentaAsync(int id)
        {
            return await _context.Venta
                .Include(v => v.Cliente)
                .Include(v => v.DetallesPedidos)
                    .ThenInclude(dp => dp.Producto)
                .FirstOrDefaultAsync(v => v.Id == id);
        }

       public async Task<bool> CrearVentaAsync(Ventas venta)
        {
            if (venta == null)
            {
                return false;
            }
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                foreach (var detalle in venta.DetallesPedidos)
                {
                    if (!await _inventarioService.ValidarExistencias(detalle.Id_Producto, detalle.Cantidad))
                    {
                        throw new InvalidOperationException($"Stock insuficiente para el producto ID: {detalle.Id_Producto}");
                    }
                }
                venta.Fecha = DateTime.Now;
                venta.Estado = EstadoVenta.Pendiente;
                _context.Venta.Add(venta);
                await _context.SaveChangesAsync();

                foreach (var detalle in venta.DetallesPedidos)
                {
                    await _inventarioService.ActualizarInventarioPorVenta(detalle);
                }
                    await transaction.CommitAsync();
                return true;
                }
                catch
                {
                    await transaction.RollbackAsync();
                    return false;
                }
                }

        public async Task<bool> ActualizarVentaAsync(Ventas venta)
        {
            if (venta == null || !await _context.Venta.AnyAsync(v => v.Id == venta.Id))
            {
                return false;
            }

            _context.Entry(venta).State = EntityState.Modified;
            
            foreach (var detalle in venta.DetallesPedidos)
            {
                _context.Entry(detalle).State = EntityState.Modified;
            }

            var result = await _context.SaveChangesAsync();
            return result > 0;
        }
    

        public async Task<bool> EliminarVentaAsync(int id)
        {
            var venta = await _context.Venta
                .Include(v => v.DetallesPedidos)
                .FirstOrDefaultAsync(v => v.Id == id);

            if (venta == null)
            {
                return false;
            }

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                _context.Venta.Remove(venta);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                return true;
            }
            catch
            {
                await transaction.RollbackAsync();
                return false;
            }
        }
    }
}