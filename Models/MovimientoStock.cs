using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Authorization;

namespace GestorComercial_API.Models
{
    public class MovimientoStock
    {
        public int Id { get; set; }
        public int ProductoId { get; set; }
        public int Cantidad { get; set; }
        public string Tipo { get; set; } = string.Empty; // "ENTRADA" o "SALIDA"
        public DateTime Fecha { get; set; } = DateTime.UtcNow;
    }

    public class Venta
    {
        public int Id { get; set; }
        public DateTime Fecha { get; set; } = DateTime.UtcNow;
        public decimal Total { get; set; }
        public List<DetalleVenta> Detalles { get; set; } = new();
    }

    public class DetalleVenta
    {
        public int Id { get; set; }
        public int VentaId { get; set; }
        public int ProductoId { get; set; }
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
    }
}