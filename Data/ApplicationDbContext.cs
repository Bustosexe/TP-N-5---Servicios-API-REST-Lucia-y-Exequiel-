using GestorComercial_API.Models;
using Microsoft.EntityFrameworkCore;

namespace GestorComercial_API.Data
{
    public class ApplicationDbContext : DbContext
    {
        // El constructor que recibe las opciones de conexión
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        // Representación de tus 7 tablas en la Base de Datos
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<Producto> Productos { get; set; }
        public DbSet<Proveedor> Proveedores { get; set; }
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<IngresoProducto> IngresosProductos { get; set; }
        public DbSet<SalidaProducto> SalidasProductos { get; set; }

        public DbSet<MovimientoStock> MovimientosStock { get; set; }
        public DbSet<Venta> Ventas { get; set; }
        public DbSet<DetalleVenta> DetallesVentas { get; set; }

        // forzamos la creación del DBContext
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=InventarioDbTP;Trusted_Connection=True;MultipleActiveResultSets=true");
            }
        }

        // Configuración adicional mediante Fluent API
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Evitar advertencias/errores en base de datos al usar decimales
            modelBuilder.Entity<Producto>()
                .Property(p => p.Precio)
                .HasColumnType("decimal(18,2)");
        }
    }
}