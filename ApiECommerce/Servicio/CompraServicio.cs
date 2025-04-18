using ProyectoFinal_PrograIII.Modelo;
using ProyectoFinal_PrograIII.Data;
using Microsoft.EntityFrameworkCore;
using ProyectoFinal_PrograIII.ApiECommerce.IServices;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProyectoFinal_PrograIII.Servicio
{
    public class CompraServicio : IComprasService
    {
        private readonly ApplicationDbContext _context;

        public CompraServicio(ApplicationDbContext context)
        {
            _context = context;
        }
        // Constructor para inyectar el servicio de inventario
        // Esto es útil para actualizar el inventario después de una compra
        private readonly IInventarioService _inventarioService;
        public CompraServicio(ApplicationDbContext context, IInventarioService inventarioService)           
        {
            _context = context;
            _inventarioService = inventarioService;
        }

        public async Task<IEnumerable<Compra>> ObtenerComprasAsync()
        {
            try
            {
                return await _context.compras
                    .Include(c => c.Proveedor)
                    .Include(c => c.DetallesCompra)
                    .AsNoTracking()
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                // Loggear el error si tienes un sistema de logging
                throw new Exception("Error al obtener las compras", ex);
            }
        }

        public async Task<Compra> ObtenerCompraAsync(int id)
        {
            return await _context.compras
                .Include(c => c.Proveedor)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<bool> CrearCompraAsync(Compra compra)
        {
            if (compra == null)
            {
                return false;
            }
             using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                compra.Fecha = DateTime.Now;
                _context.compras.Add(compra);
                await _context.SaveChangesAsync();

                foreach (var detalle in compra.DetallesCompra)
                {
                    await _inventarioService.ActualizarInventarioPorCompra(detalle);
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

        public async Task<bool> ActualizarCompraAsync(Compra compra)
        {
            if (compra == null || !await _context.compras.AnyAsync(c => c.Id == compra.Id))
            {
                return false;
            }

            _context.Entry(compra).State = EntityState.Modified;
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }

        public async Task<bool> EliminarCompraAsync(int id)
        {
            var compra = await _context.compras.FindAsync(id);
            if (compra == null)
            {
                return false;
            }

            _context.compras.Remove(compra);
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }
    }
}