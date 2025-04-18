using ProyectoFinal_PrograIII.Modelo;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProyectoFinal_PrograIII.ApiECommerce.IServices
{
    public interface IVentasService
    {
        Task<IEnumerable<Ventas>> ObtenerVentasAsync();
        Task<Ventas> ObtenerVentaAsync(int id);
        Task<bool> CrearVentaAsync(Ventas venta);
        Task<bool> ActualizarVentaAsync(Ventas venta);  
        Task<bool> EliminarVentaAsync(int id);
    }
}