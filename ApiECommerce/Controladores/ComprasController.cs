using Microsoft.AspNetCore.Mvc; // Para ControllerBase, RouteAttribute, ApiControllerAttribute, ActionResult, IActionResult, etc.
using ProyectoFinal_PrograIII.Modelo; // Para tus modelos (asegúrate de que el namespace sea correcto)
using ProyectoFinal_PrograIII.Data;  // Para ApplicationDbContext (si lo inyectas directamente en el controlador)
using ProyectoFinal_PrograIII.Servicio; // Si estás usando una capa de servicios
using ProyectoFinal_PrograIII.ApiECommerce.IServices;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProyectoFinal_PrograIII.Controladores
{
    [Route("api/[controller]")]
    [ApiController]
    public class ComprasController : ControllerBase
    {
       private readonly IComprasServicio _comprasServicio;

    public ComprasController(IComprasServicio comprasServicio)
        {
            _comprasServicio = comprasServicio;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Compra>>> GetCompras()
        {
            var compras = await _comprasServicio.ObtenerComprasAsync();
            return Ok(compras);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Compra>> GetCompra(int id)
        {
            var compra = await _comprasServicio.ObtenerComprasAsync(id);
            if (compra == null)
            {
                return NotFound();
            }
            return Ok(compra);
        }

        [HttpPost]
        public async Task<ActionResult<Compra>> CrearCompras([FromBody] Compra compra)
        {
            try
            {
                if (compra == null)
                {
                    return BadRequest("Datos de compra inválidos");
                }

                // Limpiar datos para nueva compra
                compra.Id = 0;
                compra.Proveedor = null;  // Importante: limpiar objeto proveedor

                if (compra.DetalleCompras != null)
                {
                    foreach (var detalle in compra.DetalleCompras)
                    {
                        detalle.Id = 0;
                        detalle.IdCompras = 0;
                        detalle.Compra = null;
                        detalle.Producto = null;
                    }
                }

                if (await _comprasServicio.CrearComprasAsync(compra))
                {
                    return CreatedAtAction(nameof(GetCompras), new { id = compra.Id }, compra);
                }

                return BadRequest("Error al crear la compra.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> ActualizarCompras(int id, [FromBody] Compra compra)
        {
            if (id != compra.Id)
            {
                return BadRequest("El ID de la compra no coincide con el ID de la ruta.");
            }

            if (await _comprasServicio.ActualizarComprasAsync(compra))
            {
                return NoContent(); // Indica que la actualización fue exitosa (sin devolver contenido)
            }
            return NotFound();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> EliminarCompra(int id)
        {
            if (await _comprasServicio.EliminarComprasAsync(id))
            {
                return NoContent();
            }
            return NotFound();
        }
    }
}