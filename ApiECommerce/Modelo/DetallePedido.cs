using System;
using System.Collections.Generic;
using ProyectoFinal_PrograIII.Modelo;
namespace ProyectoFinal_PrograIII.Modelo
{
    public class DetallePedido    {
        public int Id { get; set; }
        public int Id_Venta { get; set; }
        public int Id_Producto { get; set; }
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
        public decimal Subtotal { get; set; }

        // Propiedades de navegación
        public virtual Ventas Venta { get; set; }
        public virtual Producto Producto { get; set; }
    }
}

