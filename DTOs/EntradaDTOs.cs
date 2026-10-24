namespace GestorComercial_API.DTOs
{
    public class RegistrarEntradaDto
    {
        public int ProductoId { get; set; }
        public int Cantidad { get; set; }
    }

    public class RegistrarVentaDto
    {
        public List<ItemCarritoDto> Items { get; set; } = new();
    }

    public class ItemCarritoDto
    {
        public int ProductoId { get; set; }
        public int Cantidad { get; set; }
    }
}