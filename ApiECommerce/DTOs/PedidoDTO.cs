using System;
using System.Collections.Generic;

namespace ProyectoFinal_PrograIII.DTOs
{
    public class PedidoDTO
    {
        public DateTime Fecha { get; set; }
        public int IdCliente { get; set; }
        public List<DetallePedidoDTO> DetallesPedido { get; set; }
    }

    public class DetallePedidoDTO
    {
        public int IdProductos { get; set; }
        public int CantidadProductos { get; set; }
        public decimal PrecioUnitario { get; set; }
        public decimal SubTotal { get; set; }
    }
}