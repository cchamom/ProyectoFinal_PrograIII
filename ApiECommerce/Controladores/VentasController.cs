using Microsoft.AspNetCore.Mvc;
using ProyectoFinal_PrograIII.Modelo;
using ProyectoFinal_PrograIII.ApiECommerce.IServices;
using System.Collections.Generic;
using System.Threading.Tasks;
using ProyectoFinal_PrograIII.Data;  // Para ApplicationDbContext (si lo inyectas directamente en el controlador)
using ProyectoFinal_PrograIII.Servicio; // Si estás usando una capa de servicios
using Microsoft.EntityFrameworkCore;

namespace ProyectoFinal_PrograIII.Controladores
{
    [Route("api/[controller]")]
    [ApiController]
    public class VentasController : ControllerBase
    {
        private readonly IVentasService _ventaService;

        public VentasController(IVentasService ventaService)
        {
            _ventaService = ventaService;
        }
        // Obtener todas las ventas
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Ventas>>> GetVentas()
        {
            try
            {
                var ventas = await _ventaService.ObtenerVentasAsync();
                return Ok(ventas);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }
        // Obtener una venta por ID
        [HttpGet("{id}")]
        public async Task<ActionResult<Ventas>> GetVenta(int id)
        {
            var venta = await _ventaService.ObtenerVentaAsync(id);
            if (venta == null)
            {
                return NotFound();
            }
            return Ok(venta);
        }

        // Crear una nueva venta    
      [HttpPost]
        public async Task<ActionResult<Ventas>> CrearVenta([FromBody] Ventas venta)
        {
            if (venta == null)
            {
                return BadRequest("La venta no puede ser nula");
            }

            venta.Fecha = DateTime.Now;
            venta.Estado = EstadoVenta.Pendiente;
            
            // Calcular totales
            decimal total = 0;
            foreach (var detalle in venta.DetallesPedidos ?? new List<DetallePedido>())
            {
                detalle.Id_Venta = venta.Id;
                detalle.Subtotal = detalle.Cantidad * detalle.PrecioUnitario;
                total += detalle.Subtotal;
            }
            venta.Total = total;

            if (await _ventaService.CrearVentaAsync(venta))
            {
                return CreatedAtAction(nameof(GetVenta), new { id = venta.Id }, venta);
            }
            return BadRequest("Error al crear la venta");
        } 
    // Actualizar una venta
        [HttpPut("{id}")]
        public async Task<IActionResult> ActualizarVenta(int id, [FromBody] Ventas venta)
        {
            if (id != venta.Id)
            {
                return BadRequest("El ID de la venta no coincide con el ID de la ruta");
            }

            if (await _ventaService.ActualizarVentaAsync(venta))
            {
                return NoContent();
            }
            return NotFound();
        }
        // Eliminar una venta
        [HttpDelete("{id}")]
        public async Task<IActionResult> EliminarVenta(int id)
        {
            if (await _ventaService.EliminarVentaAsync(id))
            {
                return NoContent();
            }
            return NotFound();
        }
    }
}