using System;
using System.Collections.Generic;
using ProyectoFinal_PrograIII.Modelo;

namespace ProyectoFinal_PrograIII.Modelo
{
    public class Ventas
    {
        public int Id { get; set; }
        public DateTime Fecha { get; set; }
        public decimal Total { get; set; }
        public  EstadoVenta Estado { get; set; }
        public int Id_Cliente { get; set; }
        public virtual Cliente Cliente { get; set; }
        public virtual ICollection<DetallePedido> DetallesPedidos { get; set; }
    
    }
}