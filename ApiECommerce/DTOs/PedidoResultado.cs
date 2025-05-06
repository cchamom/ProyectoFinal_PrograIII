namespace ProyectoFinal_PrograIII.DTOs
{
    public class PedidoResultado
    {
        public bool Exito { get; set; }
        public string Mensaje { get; set; }
        public int? PedidoId { get; set; }
        public decimal? Total { get; set; }
    }
}