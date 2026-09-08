using GestorComercial_API.Data;
using GestorComercial_API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GestorComercial_API.Data; 
using GestorComercial_API.Models;



namespace GestorComercial_API.Controllers
{
    [Authorize] 
    [Route("api/[controller]")]
    [ApiController]
    public class ProductosController : ControllerBase

    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _env;

        public ProductosController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult> GetProductos([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            if (page < 1) page = 1;
            if (pageSize < 1) pageSize = 10;

            var totalItems = await _context.Productos.CountAsync();

            var productos = await _context.Productos
                .Include(p => p.Categoria) // Devuelve los datos de la categoría asociada
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return Ok(new
            {
                TotalItems = totalItems,
                Page = page,
                PageSize = pageSize,
                TotalPages = (int)Math.Ceiling(totalItems / (double)pageSize),
                Items = productos
            });
        }


        //POST wwwroot/api/productos
        [HttpPost("{id}/imagen")]
        public async Task<IActionResult> SubirImagen(int id, IFormFile imagen)
        {
            var producto = await _context.Productos.FindAsync(id);
            if (producto == null) return NotFound("Producto no encontrado.");

            if (imagen == null || imagen.Length == 0)
                return BadRequest("No se proporcionó ninguna imagen válida.");

            // Asegurar que la ruta wwwroot/uploads exista
            string wwwRootPath = _env.WebRootPath ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
            string uploadsFolder = Path.Combine(wwwRootPath, "uploads");

            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            // Generar nombre único y guardar físicamente el archivo
            string uniqueFileName = Guid.NewGuid().ToString() + "_" + imagen.FileName;
            string filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await imagen.CopyToAsync(fileStream);
            }

            // Actualizar el string en PostgreSQL y guardar cambios
            producto.ImagenUrl = "/uploads/" + uniqueFileName;
            await _context.SaveChangesAsync();

            return Ok(new
            {
                mensaje = "Imagen subida exitosamente",
                url = producto.ImagenUrl
            });
        }
    }
}
    
