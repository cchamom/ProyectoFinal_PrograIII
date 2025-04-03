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

        public async Task<IEnumerable<Compra>> ObtenerComprasAsync()
        {
            return await _context.compras
                .Include(c => c.Proveedor)
                .ToListAsync();
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

            compra.Fecha = DateTime.Now; // Establecer la fecha actual si no está establecida
            _context.compras.Add(compra);
            var result = await _context.SaveChangesAsync();
            return result > 0;
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