namespace ApiECommerce.Modelo
{
    public class Producto
    {
        public int Id { get; set; }
        public required string Nombre { get; set; }
        public decimal Precio { get; set; }
        public required int Existencias { get; set; }
    }
}