using GestorComercial_API.Models;
using Microsoft.EntityFrameworkCore;
using GestorComercial_API.Models; // Cambia "TuProyecto" por el namespace real de tu API

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

        // Configuración adicional mediante Fluent API
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Evitar advertencias/errores en base de datos al usar decimales
            modelBuilder.Entity<Producto>()
                .Property(p => p.Precio)
                .HasColumnType("decimal(18,2)");

            // (Opcional) Si Ingreso o Salida tienen montos totales, haz lo mismo:
            // modelBuilder.Entity<IngresoProducto>().Property(i => i.Total).HasColumnType("decimal(18,2)");
        }
    }
}