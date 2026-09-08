using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using GestorComercial_API.Models;
using Microsoft.AspNetCore.Authorization;

namespace GestorComercial_API.Models
{
    [Authorize]
    public class IngresoProducto
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public DateTime FechaRegistro { get; set; } = DateTime.UtcNow;

        [Required]
        public int Cantidad { get; set; }

        // Relaciones
        [Required]
        public int ProveedorId { get; set; }
        [ForeignKey("ProveedorId")]
        public Proveedor? Proveedor { get; set; }

        [Required]
        public int ProductoId { get; set; }
        [ForeignKey("ProductoId")]
        public Producto? Producto { get; set; }
    }
}