using ProyectoFinal_PrograIII.Modelo;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProyectoFinal_PrograIII.ApiECommerce.IServices
{
    public interface IComprasService
    {
        Task<IEnumerable<Compra>> ObtenerComprasAsync();
        Task<Compra> ObtenerCompraAsync(int id);
        Task<bool> CrearCompraAsync(Compra compra);
        Task<bool> ActualizarCompraAsync(Compra compra);
        Task<bool> EliminarCompraAsync(int id);
    }
}