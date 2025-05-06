using System;
using System.Collections.Generic;
using ProyectoFinal_PrograIII.Modelo;
using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProyectoFinal_PrograIII.Modelo
{
    public class Compra
    { 
        public int Id { get; set; }
        public DateTime Fecha { get; set; }
        [ForeignKey("proveedor")]
        public int IdProveedor { get; set; } // Clave foránea
        public double  Total {get; set;}
     
        public string Estado {get; set;}
        [JsonIgnore]
       public virtual Proveedor? Proveedor { get; set; } // Propiedad de navegación
        public virtual ICollection<DetalleCompra>? DetalleCompras { get; set; }

      
    }
}

