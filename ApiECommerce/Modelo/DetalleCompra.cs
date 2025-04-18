using System;
using ProyectoFinal_PrograIII.Modelo;
namespace ProyectoFinal_PrograIII.Modelo
{
    public class DetalleCompra
    {
        public int Id { get; set; }
        public int Id_Compras { get; set; } // Clave foránea
        public int Id_Productos { get; set; } // Clave foránea
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
        public decimal Subtotal { get; set; }
        
        // Cambiamos Compra por Compras para qsue coincida con el nombre de la clase
        public Compra Compra { get; set; }
        public Producto Producto { get; set; } // Propiedad de navegación
    }
}