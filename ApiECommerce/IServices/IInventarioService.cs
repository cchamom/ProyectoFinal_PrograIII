using ProyectoFinal_PrograIII.Modelo;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProyectoFinal_PrograIII.ApiECommerce.IServices
{
    public interface IInventarioService
    {
        Task<bool> ActualizarInventarioPorCompra(DetalleCompra detalleCompra);
        Task<bool> ActualizarInventarioPorVenta(DetallePedido detallePedido);
        Task<bool> ValidarExistencias(int productoId, int cantidad);
        Task<byte[]> GenerarExcelCompras();
        Task<byte[]> GenerarExcelVentas();
        Task<decimal> ObtenerValorInventarioTotal();
        Task<IEnumerable<MovimientoInventario>> ObtenerMovimientosPorProducto(int productoId);
        Task<byte[]> GenerarExcelCompras(DateTime? fechaInicio = null, DateTime? fechaFin = null);
        Task<byte[]> GenerarExcelVentas(DateTime? fechaInicio = null, DateTime? fechaFin = null);

    }
}