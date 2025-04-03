using Microsoft.AspNetCore.Mvc; // Para ControllerBase, RouteAttribute, ApiControllerAttribute, ActionResult, IActionResult, etc.
using ProyectoFinal_PrograIII.Modelo; // Para tus modelos (asegúrate de que el namespace sea correcto)
using ProyectoFinal_PrograIII.Data;  // Para ApplicationDbContext (si lo inyectas directamente en el controlador)
using ProyectoFinal_PrograIII.Servicio; // Si estás usando una capa de servicios
using ProyectoFinal_PrograIII.ApiECommerce.IServices;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace ProyectoFinal_PrograIII.Controladores
{
    [Route("api/[controller]")]
    [ApiController]
    public class ComprasController : ControllerBase
    {
        private readonly ProyectoFinal_PrograIII.ApiECommerce.IServices.IComprasService _comprasService;

        public ComprasController(ProyectoFinal_PrograIII.ApiECommerce.IServices.IComprasService comprasService)
        {
            _comprasService = comprasService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Compra>>> GetCompras()
        {
            var compras = await _comprasService.ObtenerComprasAsync();
            return Ok(compras);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Compra>> GetCompra(int id)
        {
            var compra = await _comprasService.ObtenerCompraAsync(id);
            if (compra == null)
            {
                return NotFound();
            }
            return Ok(compra);
        }

        [HttpPost]
        public async Task<ActionResult<Compra>> CrearCompra([FromBody] Compra compra)
        {
            if (await _comprasService.CrearCompraAsync(compra))
            {
                return CreatedAtAction(nameof(GetCompra), new { id = compra.Id }, compra);
            }
            return BadRequest("Error al crear la compra.");
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> ActualizarCompra(int id, [FromBody] Compra compra)
        {
            if (id != compra.Id)
            {
                return BadRequest("El ID de la compra no coincide con el ID de la ruta.");
            }

            if (await _comprasService.ActualizarCompraAsync(compra))
            {
                return NoContent();
            }
            return NotFound();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> EliminarCompra(int id)
        {
            if (await _comprasService.EliminarCompraAsync(id))
            {
                return NoContent();
            }
            return NotFound();
        }
    }
}