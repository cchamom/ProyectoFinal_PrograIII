using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProyectoFinal_PrograIII.Data;
using ProyectoFinal_PrograIII.Modelo;
using ProyectoFinal_PrograIII.IServices;

namespace ProyectoFinal_PrograIII.Servicio
{
    public class ProductoServicio : IProductoService
    {
        private readonly ApplicationDbContext _context;

        public ProductoServicio(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Producto>> ObtenerProductosAsync()
        {
            return await _context.Productos.ToListAsync();
        }

        public async Task<Producto> ObtenerProductoAsync(int id)
        {
            return await _context.Productos.FindAsync(id);
        }

        public async Task<bool> CrearProductoAsync(Producto producto)
        {
            if (producto == null)
            {
                return false;
            }

            _context.Productos.Add(producto);
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }

        public async Task<bool> ActualizarProductoAsync(Producto producto)
        {
            if (producto == null || !await _context.Productos.AnyAsync(p => p.Id == producto.Id))
            {
                return false;
            }

            _context.Entry(producto).State = EntityState.Modified;
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }

        public async Task<bool> EliminarProductoAsync(int id)
        {
            var producto = await _context.Productos.FindAsync(id);
            if (producto == null)
            {
                return false;
            }

            _context.Productos.Remove(producto);
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }
    }
}