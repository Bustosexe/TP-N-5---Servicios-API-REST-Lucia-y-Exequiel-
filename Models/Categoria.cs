using System.Collections.Generic;
using GestorComercial_API.Models;
using Microsoft.AspNetCore.Authorization;

namespace GestorComercial_API.Models
{
    [Authorize]
    public class Categoria
    {
       
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }

        // Propiedad de navegación: Una categoría puede tener muchos productos
        public ICollection<Producto> Productos { get; set; } = new List<Producto>();
    }
}