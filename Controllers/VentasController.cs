using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GestorComercial_API.Data;
using GestorComercial_API.Models;
using GestorComercial_API.DTOs;
using Microsoft.AspNetCore.Authorization;


namespace GestorComercial_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class VentasController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public VentasController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost("entrada")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> RegistrarEntrada([FromBody] RegistrarEntradaDto dto)
        {
            var producto = await _context.Productos.FindAsync(dto.ProductoId);
            if (producto == null) return NotFound("Producto no encontrado.");

            producto.Stock += dto.Cantidad;

            var movimiento = new MovimientoStock
            {
                ProductoId = dto.ProductoId,
                Cantidad = dto.Cantidad,
                Tipo = "ENTRADA"
            };

            _context.MovimientosStock.Add(movimiento);
            await _context.SaveChangesAsync();

            return Ok(new { Mensaje = "Stock actualizado correctamente", StockActual = producto.Stock });
        }

        [HttpPost("registrar")]
        public async Task<IActionResult> ProcesarVenta([FromBody] RegistrarVentaDto dto)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var venta = new Venta { Total = 0 };
                _context.Ventas.Add(venta);
                await _context.SaveChangesAsync(); // Genera el ID de la venta

                foreach (var item in dto.Items)
                {
                    var producto = await _context.Productos.FindAsync(item.ProductoId);
                    if (producto == null || producto.Stock < item.Cantidad)
                    {
                        return BadRequest($"Stock insuficiente para el producto ID {item.ProductoId}.");
                    }

                    // Restar stock y calcular total
                    producto.Stock -= item.Cantidad;
                    venta.Total += producto.Precio * item.Cantidad;

                    // Registrar detalle
                    _context.DetallesVentas.Add(new DetalleVenta
                    {
                        VentaId = venta.Id,
                        ProductoId = producto.Id,
                        Cantidad = item.Cantidad,
                        PrecioUnitario = producto.Precio
                    });

                    // Registrar movimiento de salida
                    _context.MovimientosStock.Add(new MovimientoStock
                    {
                        ProductoId = producto.Id,
                        Cantidad = item.Cantidad,
                        Tipo = "SALIDA"
                    });
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return Ok(new { Mensaje = "Venta procesada con éxito", VentaId = venta.Id, Total = venta.Total });
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                return StatusCode(500, "Error interno al procesar la venta.");
            }
        }
    }
}