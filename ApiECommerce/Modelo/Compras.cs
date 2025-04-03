
using System.Collections.Generic;
using ProyectoFinal_PrograIII.Modelo;

namespace ProyectoFinal_PrograIII.Modelo
{
    public class Compra
    {
        public int Id { get; set; }
        public DateTime Fecha { get; set; }
        public int Id_Proveedor { get; set; } // Clave foránea
        // Otras propiedades de la compra
        public string Estado { get; set; }
        public Proveedor Proveedor { get; set; } // Propiedad de navegación
        public ICollection<DetalleCompra> DetallesCompra { get; set; }
        public decimal Total { get; set; } // Propiedad para almacenar el total de la compra

    }
}