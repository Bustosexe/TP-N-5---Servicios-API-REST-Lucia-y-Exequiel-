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
    public class IngresosProductosController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public IngresosProductosController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<ActionResult<IngresoProducto>> PostIngreso(IngresoProducto ingreso)
        {
            // 1. Buscar el producto
            var producto = await _context.Productos.FindAsync(ingreso.ProductoId);
            if (producto == null)
            {
                return NotFound("El producto especificado no existe.");
            }

            // 2. Validar que el proveedor exista
            var proveedor = await _context.Proveedores.FindAsync(ingreso.ProveedorId);
            if (proveedor == null)
            {
                return BadRequest("El proveedor especificado no existe.");
            }

            // 3. Sumar el stock y setear la fecha
            producto.Stock += ingreso.Cantidad;
            ingreso.FechaRegistro = DateTime.UtcNow;

            // 4. Guardar en base de datos
            _context.IngresosProductos.Add(ingreso);
            await _context.SaveChangesAsync();

            return Ok(ingreso);
        }
    }
}