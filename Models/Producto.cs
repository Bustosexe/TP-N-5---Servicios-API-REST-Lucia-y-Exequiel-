using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestorComercial_Movil.Models
{
    public class Producto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public decimal Precio { get; set; }
        public int Stock { get; set; }
        public string? ImagenUrl { get; set; }
        public int CategoriaId { get; set; }
    }

    // Clase auxiliar para recibir el JSON paginado que armamos en el TP5
    public class PaginacionResponse<T>
    {
        public List<T> Items { get; set; } = new();
    }
}