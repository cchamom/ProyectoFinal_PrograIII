using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProyectoFinal_PrograIII.Modelo
{
    public class Pedido
    {

        public int Id { get; set; }
        public DateTime Fecha { get; set; }

        [ForeignKey("Cliente")]
        public int IdCliente { get; set; }

        public double Total { get; set; }
        public string Estado { get; set; }
        [JsonIgnore]
        public Cliente Cliente { get; set; }

        public ICollection<DetallePedido> DetallesPedido { get; set; }

    }
}