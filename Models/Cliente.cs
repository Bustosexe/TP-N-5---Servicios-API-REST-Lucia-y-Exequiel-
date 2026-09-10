using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;

namespace GestorComercial_API.Models
{
    [Authorize]
    public class Cliente
    {

        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Nombre { get; set; } = string.Empty;

        [Required]
        [MaxLength(20)]
        public string DniCuit { get; set; } = string.Empty;
    }
}