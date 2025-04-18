using System.Collections.Generic;
using ProyectoFinal_PrograIII.Modelo;

namespace ProyectoFinal_PrograIII.Modelo
{
    public class Producto
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public int  Existencias { get; set; }
        // Otras propiedades del producto

        public ICollection<DetallePedido> DetallesPedidos { get; set; }

        public ICollection<DetalleCompra> DetallesCompra { get; set; }
    }
}