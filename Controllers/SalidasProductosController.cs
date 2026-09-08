using GestorComercial_API.Data;
using GestorComercial_API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GestorComercial_API.Data; // Reemplaza con tu namespace
using GestorComercial_API.Models;

namespace GestorComercial_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SalidasProductosController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public SalidasProductosController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<ActionResult<SalidaProducto>> PostSalida(SalidaProducto salida)
        {
            // 1. Buscar el producto en la base de datos
            var producto = await _context.Productos.FindAsync(salida.ProductoId);

            if (producto == null)
            {
                return NotFound("El producto especificado no existe.");
            }

            // 2. Validar que haya stock suficiente (Regla del TP)
            if (producto.Stock < salida.Cantidad)
            {
                return BadRequest($"Stock insuficiente. Stock actual: {producto.Stock}");
            }

            // 3. Validar que el cliente exista
            var cliente = await _context.Clientes.FindAsync(salida.ClienteId);
            if (cliente == null)
            {
                return BadRequest("El cliente especificado no existe.");
            }

            // 4. Actualizar el stock y setear la fecha
            producto.Stock -= salida.Cantidad;
            salida.FechaRegistro = DateTime.UtcNow; // Registramos la fecha de la operación

            // 5. Guardar los cambios de forma transaccional
            _context.SalidasProductos.Add(salida);
            await _context.SaveChangesAsync();

            return Ok(salida);
        }
    }
}