using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ProyectoFinal_PrograIII.Modelo;


namespace ProyectoFinal_PrograIII.Modelo
{
    public class Producto
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public required string Nombre { get; set; }
        [Required]
        public decimal Precio { get; set; }
        public required int Existencias { get; set; }
    }
}