using Microsoft.AspNetCore.Mvc;
using ProyectoFinal_PrograIII.ApiECommerce.IServices;
using ProyectoFinal_PrograIII.Modelo;
using System.Collections.Generic;
using System.Threading.Tasks;
using ClosedXML.Excel;
using System.IO;

[ApiController]
[Route("api/[controller]")]
public class InventarioController : ControllerBase
{
    private readonly IInventarioService _inventarioService;

    public InventarioController(IInventarioService inventarioService)
    {
        _inventarioService = inventarioService;
    }
    [HttpGet("compras/excel")]
    public async Task<IActionResult> DescargarExcelCompras(
        [FromQuery] DateTime? fechaInicio = null,
        [FromQuery] DateTime? fechaFin = null)
    {
        try
        {
            var excelBytes = await _inventarioService.GenerarExcelCompras(fechaInicio, fechaFin);
            return File(
                excelBytes,
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                $"Reporte_Compras_{DateTime.Now:yyyyMMdd}.xlsx"
            );
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error al generar Excel: {ex.Message}");
        }
    }

    [HttpGet("Pedidos/excel")]
    public async Task<IActionResult> DescargarExcelVentas(
        [FromQuery] DateTime? fechaInicio = null,
        [FromQuery] DateTime? fechaFin = null)
    {
        try
        {
            var excelBytes = await _inventarioService.GenerarExcelVentas(fechaInicio, fechaFin);
            return File(
                excelBytes,
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                $"Reporte_Ventas_{DateTime.Now:yyyyMMdd}.xlsx"
            );
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error al generar Excel: {ex.Message}");
        }
    }
}
