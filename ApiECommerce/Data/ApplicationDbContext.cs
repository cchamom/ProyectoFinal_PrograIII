using Microsoft.EntityFrameworkCore;
using ProyectoFinal_PrograIII.Modelo; // Asegúrate de que la ruta sea correcta

namespace ProyectoFinal_PrograIII.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Cliente> clientes { get; set; }
        public DbSet<Proveedor> proveedores { get; set; }
        public DbSet<Producto> productos { get; set; }
        public DbSet<Compra> compras { get; set; }
        public DbSet<Pedido> pedidos { get; set; }
        public DbSet<DetallePedido> detallePedido { get; set; }
        public DbSet<DetalleCompra> detalleCompras { get; set; }
        public DbSet<Categoria> categorias { get; set; }
        public DbSet<MovimientoInventario> MovimientosInventario { get; set; } 

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuración de relaciones (Fluent API)
            modelBuilder.Entity<Compra>()
                .HasOne(c => c.Proveedor)
                .WithMany()
                .HasForeignKey(c => c.IdProveedor)
                .IsRequired();
            
            // Configuración de la relación Compra-DetalleCompra
            modelBuilder.Entity<DetalleCompra>()
                .HasOne(d => d.Compra)
                .WithMany(c => c.DetalleCompras)
                .HasForeignKey(d => d.IdCompras)
                .OnDelete(DeleteBehavior.Cascade);

    // Configuración de la relación DetalleCompra-Producto
            modelBuilder.Entity<DetalleCompra>()
                .HasOne(d => d.Producto)
                .WithMany()
                .HasForeignKey(d => d.IdProductos)
                .OnDelete(DeleteBehavior.Cascade);
            
            modelBuilder.Entity<DetallePedido>()
                .HasOne(d => d.Pedido)
                .WithMany(p => p.DetallesPedido)
                .HasForeignKey(d => d.IdPedidos)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<DetallePedido>()
                .HasOne(d => d.Producto)
                .WithMany()
                .HasForeignKey(d => d.IdProductos)
                .OnDelete(DeleteBehavior.Cascade);
             modelBuilder.Entity<MovimientoInventario>(entity =>
            {
                // Configuración de la relación con Producto
                entity.HasOne(m => m.Producto)
                    .WithMany()
                    .HasForeignKey(m => m.IdProductos)
                    .OnDelete(DeleteBehavior.Cascade);

                // Configuración de la relación con Compra
                entity.HasOne(m => m.Compra)
                    .WithMany()
                    .HasForeignKey(m => m.IdCompras)
                    .OnDelete(DeleteBehavior.SetNull);

                // Configuración de la relación con Pedido
                entity.HasOne(m => m.Pedido)
                    .WithMany()
                    .HasForeignKey(m => m.IdPedidos)
                    .OnDelete(DeleteBehavior.SetNull);

                // Configuración de campos decimales
                entity.Property(m => m.PrecioUnitario)
                    .HasColumnType("decimal(18,2)")
                    .IsRequired();  
                entity.Property(m => m.SubTotal)
                    .HasColumnType("decimal(18,2)")
                    .IsRequired();
            });
        }
    }
}