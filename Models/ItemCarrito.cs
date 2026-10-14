using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestorComercial_Movil.Models
{
    public class ItemCarrito
    {
        public Producto Producto { get; set; } = new Producto();
        public int Cantidad { get; set; }
        public decimal Subtotal => Producto.Precio * Cantidad;
    }

    // También necesitamos replicar el modelo para enviar la venta a tu API
    public class SalidaProductoRequest
    {
        public int ProductoId { get; set; }
        public int ClienteId { get; set; }
        public int Cantidad { get; set; }
    }
}