using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Authorization;

namespace GestorComercial_API.Models
{
    [Authorize]
    public class Producto
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Nombre { get; set; } = string.Empty;

        [Required]
        public string Descripcion { get; set; } = string.Empty;

        [Required]
        public decimal Precio { get; set; }

        [Required]
        public int Stock { get; set; }

        public string? ImagenUrl { get; set; }

        // Relación con Categoría (N:1)
        [Required]
        public int CategoriaId { get; set; }
        [ForeignKey("CategoriaId")]
        public Categoria? Categoria { get; set; }

        // Relaciones con movimientos (1:N)
        public ICollection<IngresoProducto> Ingresos { get; set; } = new List<IngresoProducto>();
        public ICollection<SalidaProducto> Salidas { get; set; } = new List<SalidaProducto>();
    }
}