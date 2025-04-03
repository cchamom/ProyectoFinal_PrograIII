using Microsoft.AspNetCore.Mvc;
using ApiECommerce.Modelo;
using ApiECommerce.IServices;

namespace ApiECommerce.Controladores
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductosController : ControllerBase
    {
        private readonly IProductoService _productoService;

        public ProductosController(IProductoService productoService)
        {
            _productoService = productoService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Producto>>> GetProductos()
        {
            var productos = await _productoService.ObtenerProductosAsync();
            return Ok(productos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Producto>> GetProducto(int id)
        {
            var producto = await _productoService.ObtenerProductoAsync(id);
            if (producto == null)
            {
                return NotFound();
            }
            return Ok(producto);
        }

        [HttpPost]
        public async Task<ActionResult<Producto>> CrearProducto([FromBody] Producto producto)
        {
            if (await _productoService.CrearProductoAsync(producto))
            {
                return CreatedAtAction(nameof(GetProducto), new { id = producto.Id }, producto);
            }
            return BadRequest("Error al crear producto.");
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> ActualizarProducto(int id, [FromBody] Producto producto)
        {
            if (id != producto.Id)
            {
                return BadRequest("El ID del producto no coincide con el ID de la ruta.");
            }

            if (await _productoService.ActualizarProductoAsync(producto))
            {
                return NoContent();
            }
            return NotFound();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> EliminarProducto(int id)
        {
            if (await _productoService.EliminarProductoAsync(id))
            {
                return NoContent();
            }
            return NotFound();
        }
    }
}