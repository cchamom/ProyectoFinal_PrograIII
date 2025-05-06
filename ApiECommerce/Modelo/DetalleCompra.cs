using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ProyectoFinal_PrograIII.Modelo
{
    public class DetalleCompra
    {
        public int Id { get; set; }
        
        [ForeignKey("Compra")]
        public int IdCompras { get; set; }
        
        [ForeignKey("Producto")]
        public int IdProductos { get; set; }
        
        public int CantidadProductos { get; set; }
        
        [Column(TypeName = "decimal(18,2)")]
        public decimal PrecioUnitario { get; set; }
        
        [Column(TypeName = "decimal(18,2)")]
        public decimal SubTotal { get; set; }

        [JsonIgnore]
        public virtual Compra? Compra { get; set; }
        
        [JsonIgnore]
        public virtual Producto? Producto { get; set; }
    }
}