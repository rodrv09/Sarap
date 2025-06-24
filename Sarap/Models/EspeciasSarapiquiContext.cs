using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Sarap.Models;

public partial class EspeciasSarapiquiContext : DbContext
{
    public EspeciasSarapiquiContext()
    {
    }

    public EspeciasSarapiquiContext(DbContextOptions<EspeciasSarapiquiContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Cliente> Clientes { get; set; }

    public virtual DbSet<Proveedore> Proveedores { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    public virtual DbSet<Producto> Productos { get; set; }

    public virtual DbSet<Empleado> Empleado { get; set; }


    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
=> optionsBuilder.UseSqlServer("Server=chrstation.database.windows.net;Database=especias_sarapiqui;User ID=chrstation;Password=chrsonic8!;Encrypt=True;TrustServerCertificate=False;");
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Cliente>(entity =>
        {
            entity.HasKey(e => e.ClienteId).HasName("PK__Clientes__71ABD0A7EF0D6633");

            entity.Property(e => e.ClienteId).HasColumnName("ClienteID");
            entity.Property(e => e.Apellido)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Direccion)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.Email)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.FechaRegistro)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Telefono)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Activo)
            .HasDefaultValue(true)
            .IsRequired();
        });

        modelBuilder.Entity<Proveedore>(entity =>
        {
            entity.HasKey(e => e.ProveedorId).HasName("PK__Proveedo__61266BB95E4FC109");

            entity.Property(e => e.ProveedorId).HasColumnName("ProveedorID");
            entity.Property(e => e.ContactoNombre)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Direccion)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.Email)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.FechaRegistro)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.NombreEmpresa)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.Telefono)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Activo)
    .HasDefaultValue(true)
    .IsRequired();

        });
        modelBuilder.Entity<Empleado>(entity =>
        {
            entity.ToTable("Empleado");

            entity.HasKey(e => e.Id).HasName("PK_Empleado");

            entity.Property(e => e.Id).HasColumnName("EmpleadoID");

            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .IsUnicode(false)
                .IsRequired();

            entity.Property(e => e.Apellidos)
                .HasMaxLength(100)
                .IsUnicode(false)
                .IsRequired();

            entity.Property(e => e.Cedula)
                .HasMaxLength(20)
                .IsUnicode(false)
                .IsRequired();

            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false);

            entity.Property(e => e.Rol)
                .HasMaxLength(50)
                .IsUnicode(false)
                .IsRequired();

            entity.Property(e => e.Activo)
                .HasDefaultValue(true)
                .IsRequired();

            entity.HasOne(e => e.Usuario)
             .WithMany()
             .HasForeignKey(e => e.UsuarioId)
             .OnDelete(DeleteBehavior.Restrict);
        });



        modelBuilder.Entity<Producto>(entity =>
        {
            entity.HasKey(e => e.ProductoId).HasName("PK__Productos__..."); 

            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .IsUnicode(false);

            entity.Property(e => e.Descripcion)
                .HasMaxLength(255)
                .IsUnicode(false);

            entity.Property(e => e.Precio)
                .HasColumnType("decimal(10, 2)");

            entity.Property(e => e.FechaRegistro)
                .HasDefaultValueSql("(getdate())")  // Ahora EF sabe que en la BD la fecha es por defecto GETDATE()
                .HasColumnType("datetime");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.UsuarioId).HasName("PK__Usuarios__2B3DE798B53331A2");

            entity.HasIndex(e => e.NombreUsuario, "UQ__Usuarios__6B0F5AE00995BCE4").IsUnique();

            entity.HasIndex(e => e.Email, "UQ__Usuarios__A9D10534FE51F390").IsUnique();

            entity.Property(e => e.UsuarioId).HasColumnName("UsuarioID");
            entity.Property(e => e.Apellido)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.ContraseñaHash)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Email)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.FechaCreacion)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.NombreUsuario)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Rol)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Telefono)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Activo)
    .HasDefaultValue(true)
    .IsRequired();

        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
