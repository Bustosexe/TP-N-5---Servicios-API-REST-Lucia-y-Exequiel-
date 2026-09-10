using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;

namespace GestorComercial_API.Models
{
    [Authorize]
    public class Proveedor
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(150)]
        public string RazonSocial { get; set; } = string.Empty;

        [MaxLength(100)]
        public string? Contacto { get; set; }
    }
}