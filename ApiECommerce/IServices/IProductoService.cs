using System.Collections.Generic;
using System.Threading.Tasks;
using ProyectoFinal_PrograIII.Modelo;

namespace ProyectoFinal_PrograIII.IServices
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