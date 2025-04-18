using System;
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
        public DbSet<Producto> Productos { get; set; }
        public DbSet<Compra> compras { get; set; }
        public DbSet<DetallePedido> DetallesPedidos { get; set; }
        public DbSet<Pedido> Pedidos { get; set; }
        public DbSet<Ventas> Venta { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
             modelBuilder.Entity<Compra>()
                .HasOne(c => c.Proveedor)
                .WithMany()
                .HasForeignKey(c => c.Id_Proveedor)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<DetalleCompra>()
                .HasOne(dc => dc.Compra)
                .WithMany(c => c.DetallesCompra)
                .HasForeignKey(dc => dc.Id_Compras)
                .OnDelete(DeleteBehavior.Cascade);
            
            modelBuilder.Entity<Ventas>()
                .HasMany(v => v.DetallesPedidos)
                .WithOne(dp => dp.Venta)
                .HasForeignKey(dp => dp.Id_Venta)
                .OnDelete(DeleteBehavior.Cascade); // Configura el comportamiento ON DELETE CASCADE

            // Configuración de Productos con DetalleVenta
            modelBuilder.Entity<DetallePedido>()
                .HasOne(dv => dv.Producto)
                .WithMany(p => p.DetallesPedidos)
                .HasForeignKey(dv => dv.Id_Producto)
                .OnDelete(DeleteBehavior.Restrict); // Configura el comportamiento ON DELETE RESTRICT
            // Configuración de relaciones (Fluent API)
            /*modelBuilder.Entity<Compra>()
                .HasOne(c => c.Proveedor)
                .WithMany(p => p.Compras)
                .HasForeignKey(c => c.Id_Proveedor);

            modelBuilder.Entity<DetallePedido>()
                .HasOne(dp => dp.Producto)
                .WithMany(pr => pr.DetallesPedido)
                .HasForeignKey(dp => dp.Id_Productos)
                .OnDelete(DeleteBehavior.Cascade); // Configura el comportamiento ON DELETE CASCADE

            modelBuilder.Entity<DetallePedido>()
                .HasOne(dp => dp.Pedido)
                .WithMany(pe => pe.DetallesPedido)
                .HasForeignKey(dp => dp.Id_Pedidos)
                .OnDelete(DeleteBehavior.Cascade); // Configura el comportamiento ON DELETE CASCADE

            modelBuilder.Entity<DetalleCompra>()
                .HasOne(dc => dc.Producto)
                .WithMany(pr => pr.DetallesCompra)
                .HasForeignKey(dc => dc.Id_Productos)
                .OnDelete(DeleteBehavior.Cascade); // Configura el comportamiento ON DELETE CASCADE

            modelBuilder.Entity<DetalleCompra>()
                .HasOne(dc => dc.Compra)
                .WithMany(co => co.DetallesCompra)
                .HasForeignKey(dc => dc.Id_Compras)
                .OnDelete(DeleteBehavior.Cascade); // Configura el comportamiento ON DELETE CASCADE
        */
        }
    }
}