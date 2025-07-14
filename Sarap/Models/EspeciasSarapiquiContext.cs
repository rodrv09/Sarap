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

    public virtual DbSet<Empleado> Empleados { get; set; }
    public virtual DbSet<RegistroHorasQuincena> RegistroHorasQuincena { get; set; }
    public virtual DbSet<PlanillaColones> PlanillaColones { get; set; }

    public virtual DbSet<VacacionesEmpleado> VacacionesEmpleado { get; set; }


    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
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

            entity.Property(e => e.Id).HasColumnName("Id");

            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .IsUnicode(false)
                .IsRequired();

            entity.Property(e => e.Identidad)
                .HasMaxLength(50)
                .IsUnicode(false)
                .IsRequired();

            entity.Property(e => e.Email)
                .HasMaxLength(150)
                .IsUnicode(false);

            entity.Property(e => e.Telefono)
                .HasMaxLength(20)
                .IsUnicode(false);

            entity.Property(e => e.Direccion)
                .HasMaxLength(250)
                .IsUnicode(false);

            entity.Property(e => e.FechaContratacion)
                .HasColumnType("datetime");

            entity.Property(e => e.DiasVacacionesDisponibles);

            entity.Property(e => e.ApellidoPaterno)
                .HasMaxLength(50);
            // Aquí no ponemos IsUnicode(false) para que sea nvarchar

            entity.Property(e => e.ApellidoMaterno)
                .HasMaxLength(50);
            // Igual

            entity.Property(e => e.SalarioHora)
                .HasColumnType("decimal(18,2)");
            entity.Property(e => e.Activo)
            .HasDefaultValue(true)
            .IsRequired();
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
                .HasDefaultValueSql("(getdate())")  
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

        modelBuilder.Entity<RegistroHorasQuincena>(entity =>
        {
            entity.ToTable("RegistroHorasQuincena");

            entity.HasKey(e => e.Id).HasName("PK_RegistroHorasQuincena");

            entity.Property(e => e.Id).HasColumnName("Id");

            entity.Property(e => e.Identidad)
                .HasMaxLength(50)
                .IsUnicode(false)
                .IsRequired();

            entity.Property(e => e.FechaInicio)
                .HasColumnType("datetime")
                .IsRequired();

            entity.Property(e => e.FechaFin)
                .HasColumnType("datetime")
                .IsRequired();

            entity.Property(e => e.HorasOrdinarias)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            entity.Property(e => e.HorasIncapacidad)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            entity.Property(e => e.HorasVacaciones)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            entity.Property(e => e.HorasFeriadoLey)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            entity.Property(e => e.HorasExtra15)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            entity.Property(e => e.HorasExtra20)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            entity.Property(e => e.HorasPermisoSinGoce)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            entity.Property(e => e.FechaRegistro)
                .HasColumnType("datetime")
                .IsRequired();
        });

        modelBuilder.Entity<PlanillaColones>(entity =>
        {
            entity.ToTable("PlanillaColones");

            entity.HasKey(e => e.Id).HasName("PK_PlanillaColones");

            entity.Property(e => e.Id).HasColumnName("Id");

            entity.Property(e => e.Identidad)
                .HasMaxLength(50)
                .IsUnicode(false)
                .IsRequired();

            entity.Property(e => e.FechaInicio)
                .HasColumnType("datetime")
                .IsRequired();

            entity.Property(e => e.FechaFin)
                .HasColumnType("datetime")
                .IsRequired();

            entity.Property(e => e.SalarioOrdinario)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            entity.Property(e => e.SalarioIncapacidad)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            entity.Property(e => e.SalarioVacaciones)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            entity.Property(e => e.SalarioFeriado)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            entity.Property(e => e.SalarioExtra15)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            entity.Property(e => e.SalarioExtra20)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            entity.Property(e => e.SalarioBruto)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            entity.Property(e => e.DeduccionCCSS)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            entity.Property(e => e.SalarioNeto)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            entity.Property(e => e.FechaRegistro)
    .HasColumnType("datetime")
    .IsRequired(false);  


            entity.Property(e => e.HorasIncapacidad)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            entity.Property(e => e.HorasVacaciones)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            entity.Property(e => e.HorasFeriadoLey)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            entity.Property(e => e.HorasExtra15)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            entity.Property(e => e.HorasExtra20)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            entity.Property(e => e.HorasPermisoSinGoce)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            entity.Property(e => e.HorasOrdinarias)
     .HasColumnType("decimal(18,2)")
     .IsRequired(false); // ✅ Ahora coincide con la tabla

        });

        modelBuilder.Entity<VacacionesEmpleado>(entity =>
        {
            entity.ToTable("VacacionesEmpleado");
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Identidad)
                .HasMaxLength(50)
                .IsRequired();

            entity.Property(e => e.NombreCompleto)
                .HasMaxLength(150)
                .IsRequired();

            entity.Property(e => e.DiasDisponibles)
                .IsRequired();

            entity.Property(e => e.DiasUsados)
                .IsRequired();

            entity.Property(e => e.FechaUltimaActualizacion)
                .HasColumnType("datetime")
                .IsRequired(false);  // Nullable datetime
        });



        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
