
using System.Collections.Generic;
using ProyectoFinal_PrograIII.Modelo;

namespace ProyectoFinal_PrograIII.Modelo
{
    public class Compra
    { public int Id { get; set; }
        public DateTime Fecha { get; set; }
        public decimal Total { get; set; }
        public string Estado { get; set; }
        public int Id_Proveedor { get; set; }

        // Evitar referencias circulares

        public virtual Proveedor Proveedor { get; set; }
        
       
        public virtual ICollection<DetalleCompra> DetallesCompra { get; set; }
    }
}