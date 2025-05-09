using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProyectoFinal_PrograIII.Modelo
{
    public class MovimientoInventario
    {
        public int Id { get; set; }
        public DateTime Fecha { get; set; }

        [ForeignKey("Producto")]
        public int IdProductos { get; set; }
        public int Cantidad { get; set; }
        public string TipoMovimiento { get; set; }  // "Entrada" o "Salida"
         public string Referencia { get; set; } 
        public int? IdCompras { get; set; }    
        public int? IdPedidos { get; set; } 

        [Required]
        [Range(0, double.MaxValue)]
        public decimal PrecioUnitario { get; set; }
        
        [Required]
        public decimal SubTotal {get;set;}
        

        // Relaciones de navegación
        public virtual Producto Producto { get; set; }
        public virtual Compra? Compra { get; set; }
        public virtual Pedido? Pedido { get; set; }
    }
}