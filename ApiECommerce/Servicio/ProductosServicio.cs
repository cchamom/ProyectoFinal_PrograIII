using ApiECommerce.Modelo;
using ApiECommerce.Data;
using Microsoft.EntityFrameworkCore;
using ApiECommerce.IServices;
// ...resto del código igual...

namespace ApiECommerce.Servicio
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
            return await _context.productos.ToListAsync();
        }

        public async Task<Producto> ObtenerProductoAsync(int id)
        {
            return await _context.productos.FindAsync(id);
        }

        public async Task<bool> CrearProductoAsync(Producto producto)
        {
            if (producto == null)
            {
                return false;
            }

            _context.productos.Add(producto);
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }

        public async Task<bool> ActualizarProductoAsync(Producto producto)
        {
            if (producto == null || !await _context.productos.AnyAsync(p => p.Id == producto.Id))
            {
                return false;
            }

            _context.Entry(producto).State = EntityState.Modified;
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }

        public async Task<bool> EliminarProductoAsync(int id)
        {
            var producto = await _context.productos.FindAsync(id);
            if (producto == null)
            {
                return false;
            }

            _context.productos.Remove(producto);
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }
    }
}