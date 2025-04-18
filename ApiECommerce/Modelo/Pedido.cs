using System;
using System.Collections.Generic;

namespace ProyectoFinal_PrograIII.Modelo
{
    public class Pedido
    {
        public int Id { get; set; }
        public DateTime Fecha { get; set; }
        public decimal Total { get; set; }
        public int Id_cliente { get; set; }
        public virtual Cliente Cliente { get; set; }
        public virtual ICollection<DetallePedido> DetallesPedido { get; set; }
    }
}