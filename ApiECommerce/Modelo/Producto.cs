using System.Collections.Generic;
using ProyectoFinal_PrograIII.Modelo;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ProyectoFinal_PrograIII.Modelo;


namespace ProyectoFinal_PrograIII.Modelo
{
    public class Producto
    {
        [Key]
        public int Id { get; set; }
        public string Nombre { get; set; }
        public int  Existencias { get; set; }
        // Otras propiedades del producto

        public ICollection<DetallePedido> DetallesPedidos { get; set; }

        public ICollection<DetalleCompra> DetallesCompra { get; set; }
        [Required]
        public required string Nombre { get; set; }
        [Required]
        public decimal Precio { get; set; }
        public required int Existencias { get; set; }
    }
}