using ApiECommerce.Modelo;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApiECommerce.IServices
{
    public interface IProductoService
    {
       Task<IEnumerable<Producto>> ObtenerProductosAsync();
        Task<Producto> ObtenerProductoAsync(int id);
        Task<bool> CrearProductoAsync(Producto producto);
        Task<bool> ActualizarProductoAsync(Producto producto);
        Task<bool> EliminarProductoAsync(int id);
    }
}