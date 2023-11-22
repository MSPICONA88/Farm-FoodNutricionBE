using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Frontend.Models;

public partial class FarmFoodNutricionContext : DbContext
{
    public FarmFoodNutricionContext()
    {
    }

    public FarmFoodNutricionContext(DbContextOptions<FarmFoodNutricionContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Alimentacione> Alimentaciones { get; set; }

    public virtual DbSet<Alimento> Alimentos { get; set; }

    public virtual DbSet<AlimentosxDietum> AlimentosxDieta { get; set; }

    public virtual DbSet<Animale> Animales { get; set; }

    public virtual DbSet<Dieta> Dietas { get; set; }

    public virtual DbSet<Especy> Especies { get; set; }

    public virtual DbSet<Finalidade> Finalidades { get; set; }

    public virtual DbSet<Lote> Lotes { get; set; }

    public virtual DbSet<Nutriente> Nutrientes { get; set; }

    public virtual DbSet<NutrientesxAlimento> NutrientesxAlimentos { get; set; }

    public virtual DbSet<PlanesAlimentacion> PlanesAlimentacions { get; set; }

    public virtual DbSet<Raza> Razas { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<StockAlimento> StockAlimentos { get; set; }

    public virtual DbSet<TiposMovimiento> TiposMovimientos { get; set; }

    public virtual DbSet<TiposTratamiento> TiposTratamientos { get; set; }

    public virtual DbSet<TratamientoLote> TratamientoLotes { get; set; }

    public virtual DbSet<TratamientosAnimal> TratamientosAnimals { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Server = localhost; Database = Farm&FoodNutricion; Port = 5432; UserId = postgres; Password = k46m6zt8;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Alimentacione>(entity =>
        {
            entity.HasKey(e => e.IdAlimentacion).HasName("Alimentaciones_pkey");

            entity.Property(e => e.IdAlimentacion)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id_alimentacion");
            entity.Property(e => e.FechaAlimentacion).HasColumnName("fecha_alimentacion");
            entity.Property(e => e.IdPlan).HasColumnName("id_plan");
            entity.Property(e => e.ToneladasDispensadas).HasColumnName("toneladas_dispensadas");

            entity.HasOne(d => d.IdPlanNavigation).WithMany(p => p.Alimentaciones)
                .HasForeignKey(d => d.IdPlan)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_id_plan");
        });

        modelBuilder.Entity<Alimento>(entity =>
        {
            entity.HasKey(e => e.IdAlimento).HasName("Alimentos_pkey");

            entity.Property(e => e.IdAlimento)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id_alimento");
            entity.Property(e => e.NombreAlimento).HasColumnName("nombre_alimento");
        });

        modelBuilder.Entity<AlimentosxDietum>(entity =>
        {
            entity.HasKey(e => e.IdAlimentoDieta).HasName("AlimentosxDieta_pkey");

            entity.Property(e => e.IdAlimentoDieta)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id_alimento_dieta");
            entity.Property(e => e.IdAlimento).HasColumnName("id_alimento");
            entity.Property(e => e.IdDieta).HasColumnName("id_dieta");
            entity.Property(e => e.Porcentaje).HasColumnName("porcentaje");

            entity.HasOne(d => d.IdAlimentoNavigation).WithMany(p => p.AlimentosxDieta)
                .HasForeignKey(d => d.IdAlimento)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_id_alimento");

            entity.HasOne(d => d.IdDietaNavigation).WithMany(p => p.AlimentosxDieta)
                .HasForeignKey(d => d.IdDieta)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_id_dieta");
        });

        modelBuilder.Entity<Animale>(entity =>
        {
            entity.HasKey(e => e.IdAnimal).HasName("Animales_pkey");

            entity.Property(e => e.IdAnimal)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id_animal");
            entity.Property(e => e.IdLote).HasColumnName("id_lote");

            entity.HasOne(d => d.IdLoteNavigation).WithMany(p => p.Animales)
                .HasForeignKey(d => d.IdLote)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_lotes");
        });

        modelBuilder.Entity<Dieta>(entity =>
        {
            entity.HasKey(e => e.IdDieta).HasName("Dietas_pkey");

            entity.Property(e => e.IdDieta)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id_dieta");
            entity.Property(e => e.FechaCreacion).HasColumnName("fecha_creacion");
            entity.Property(e => e.NombreDieta).HasColumnName("nombre_dieta");
            entity.Property(e => e.Observacion).HasColumnName("observacion");
        });

        modelBuilder.Entity<Especy>(entity =>
        {
            entity.HasKey(e => e.IdEspecie).HasName("Especies_pkey");

            entity.Property(e => e.IdEspecie)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id_especie");
            entity.Property(e => e.NombreEspecie).HasColumnName("nombre_especie");
        });

        modelBuilder.Entity<Finalidade>(entity =>
        {
            entity.HasKey(e => e.IdFinalidad).HasName("Finalildades_pkey");

            entity.Property(e => e.IdFinalidad)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id_finalidad");
            entity.Property(e => e.Descripcion).HasColumnName("descripcion");
            entity.Property(e => e.NombreFinalidad).HasColumnName("nombre_finalidad");
        });

        modelBuilder.Entity<Lote>(entity =>
        {
            entity.HasKey(e => e.IdLote).HasName("Lotes_pkey");

            entity.Property(e => e.IdLote)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id_lote");
            entity.Property(e => e.CantidadAnimales).HasColumnName("cantidad_animales");
            entity.Property(e => e.EdadMeses).HasColumnName("edad_meses");
            entity.Property(e => e.FechaIngreso).HasColumnName("fecha_ingreso");
            entity.Property(e => e.IdFinalidad).HasColumnName("id_finalidad");
            entity.Property(e => e.IdRaza).HasColumnName("id_raza");
            entity.Property(e => e.PesoTotal).HasColumnName("peso_total");

            entity.HasOne(d => d.IdFinalidadNavigation).WithMany(p => p.Lotes)
                .HasForeignKey(d => d.IdFinalidad)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_id_finalidad");

            entity.HasOne(d => d.IdRazaNavigation).WithMany(p => p.Lotes)
                .HasForeignKey(d => d.IdRaza)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_id_raza");
        });

        modelBuilder.Entity<Nutriente>(entity =>
        {
            entity.HasKey(e => e.IdNutriente).HasName("Nutrientes_pkey");

            entity.Property(e => e.IdNutriente)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id_nutriente");
            entity.Property(e => e.NombreNutriente).HasColumnName("nombre_nutriente");
        });

        modelBuilder.Entity<NutrientesxAlimento>(entity =>
        {
            entity.HasKey(e => e.IdNutrientexalimento).HasName("NutrientesxAlimento_pkey");

            entity.ToTable("NutrientesxAlimento");

            entity.Property(e => e.IdNutrientexalimento)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id_nutrientexalimento");
            entity.Property(e => e.IdAlimento).HasColumnName("id_alimento");
            entity.Property(e => e.IdNutriente).HasColumnName("id_nutriente");
            entity.Property(e => e.Porcentaje).HasColumnName("porcentaje");

            entity.HasOne(d => d.IdAlimentoNavigation).WithMany(p => p.NutrientesxAlimentos)
                .HasForeignKey(d => d.IdAlimento)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_alimento");

            entity.HasOne(d => d.IdNutrienteNavigation).WithMany(p => p.NutrientesxAlimentos)
                .HasForeignKey(d => d.IdNutriente)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_nutriente");
        });

        modelBuilder.Entity<PlanesAlimentacion>(entity =>
        {
            entity.HasKey(e => e.IdPlan).HasName("Planes_alimentacion_pkey");

            entity.ToTable("PlanesAlimentacion");

            entity.Property(e => e.IdPlan)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id_plan");
            entity.Property(e => e.CantToneladaDiaria).HasColumnName("cant_tonelada_diaria");
            entity.Property(e => e.FechaFin).HasColumnName("fecha_fin");
            entity.Property(e => e.FechaInicio).HasColumnName("fecha_inicio");
            entity.Property(e => e.IdDieta).HasColumnName("id_dieta");
            entity.Property(e => e.IdLote).HasColumnName("id_lote");

            entity.HasOne(d => d.IdDietaNavigation).WithMany(p => p.PlanesAlimentacions)
                .HasForeignKey(d => d.IdDieta)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_id_dieta");

            entity.HasOne(d => d.IdLoteNavigation).WithMany(p => p.PlanesAlimentacions)
                .HasForeignKey(d => d.IdLote)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_id_lote");
        });

        modelBuilder.Entity<Raza>(entity =>
        {
            entity.HasKey(e => e.IdRaza).HasName("Razas_pkey");

            entity.Property(e => e.IdRaza)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id_raza");
            entity.Property(e => e.IdEspecie).HasColumnName("id_especie");
            entity.Property(e => e.NombreRaza).HasColumnName("nombre_raza");

            entity.HasOne(d => d.IdEspecieNavigation).WithMany(p => p.Razas)
                .HasForeignKey(d => d.IdEspecie)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_id_especie");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.IdRol).HasName("Roles_pkey");

            entity.Property(e => e.IdRol)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id_rol");
            entity.Property(e => e.NombreRol).HasColumnName("nombre_rol");
        });

        modelBuilder.Entity<StockAlimento>(entity =>
        {
            entity.HasKey(e => e.IdStock).HasName("StockAlimentos_pkey");

            entity.Property(e => e.IdStock)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id_stock");
            entity.Property(e => e.FechaRegistro).HasColumnName("fecha_registro");
            entity.Property(e => e.IdAlimento).HasColumnName("id_alimento");
            entity.Property(e => e.IdTipoMovimiento).HasColumnName("id_tipo_movimiento");
            entity.Property(e => e.PrecioTonelada)
                .HasColumnType("money")
                .HasColumnName("precio_tonelada");
            entity.Property(e => e.Toneladas).HasColumnName("toneladas");

            entity.HasOne(d => d.IdAlimentoNavigation).WithMany(p => p.StockAlimentos)
                .HasForeignKey(d => d.IdAlimento)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_id_alimento");

            entity.HasOne(d => d.IdTipoMovimientoNavigation).WithMany(p => p.StockAlimentos)
                .HasForeignKey(d => d.IdTipoMovimiento)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_tipo_mov");
        });

        modelBuilder.Entity<TiposMovimiento>(entity =>
        {
            entity.HasKey(e => e.IdMovimiento).HasName("Tipos_movimiento_pkey");

            entity.ToTable("TiposMovimiento");

            entity.Property(e => e.IdMovimiento)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id_movimiento");
            entity.Property(e => e.Descripcion).HasColumnName("descripcion");
            entity.Property(e => e.NombreMovimiento).HasColumnName("nombre_movimiento");
        });

        modelBuilder.Entity<TiposTratamiento>(entity =>
        {
            entity.HasKey(e => e.IdTipoTrat).HasName("TiposTratamiento_pkey");

            entity.ToTable("TiposTratamiento");

            entity.Property(e => e.IdTipoTrat)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id_tipo_trat");
            entity.Property(e => e.Decripcion).HasColumnName("decripcion");
        });

        modelBuilder.Entity<TratamientoLote>(entity =>
        {
            entity.HasNoKey();

            entity.Property(e => e.FechaFin).HasColumnName("fecha_fin");
            entity.Property(e => e.FechaInicio).HasColumnName("fecha_inicio");
            entity.Property(e => e.IdLote).HasColumnName("id_lote");
            entity.Property(e => e.IdTipoTrat).HasColumnName("id_tipo_trat");
            entity.Property(e => e.IdTratLote)
                .ValueGeneratedOnAdd()
                .UseIdentityAlwaysColumn()
                .HasColumnName("id_trat_lote");
            entity.Property(e => e.Medicacion).HasColumnName("medicacion");

            entity.HasOne(d => d.IdLoteNavigation).WithMany()
                .HasForeignKey(d => d.IdLote)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_id_lote");

            entity.HasOne(d => d.IdTipoTratNavigation).WithMany()
                .HasForeignKey(d => d.IdTipoTrat)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_id_tipo_trat");
        });

        modelBuilder.Entity<TratamientosAnimal>(entity =>
        {
            entity.HasKey(e => e.IdTratAnimal).HasName("TratamientosAnimal_pkey");

            entity.ToTable("TratamientosAnimal");

            entity.Property(e => e.IdTratAnimal)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id_trat_animal");
            entity.Property(e => e.FechaFin).HasColumnName("fecha_fin");
            entity.Property(e => e.FechaInicio).HasColumnName("fecha_inicio");
            entity.Property(e => e.IdAnimal).HasColumnName("id_animal");
            entity.Property(e => e.IdTipoTrat).HasColumnName("id_tipo_trat");
            entity.Property(e => e.Medicacion).HasColumnName("medicacion");

            entity.HasOne(d => d.IdAnimalNavigation).WithMany(p => p.TratamientosAnimals)
                .HasForeignKey(d => d.IdAnimal)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_id_animal");

            entity.HasOne(d => d.IdTipoTratNavigation).WithMany(p => p.TratamientosAnimals)
                .HasForeignKey(d => d.IdTipoTrat)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_id_tipo_trat");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.IdUsuario).HasName("Usuarios_pkey");

            entity.Property(e => e.IdUsuario)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id_usuario");
            entity.Property(e => e.Email).HasColumnName("email");
            entity.Property(e => e.FechaCreacion).HasColumnName("fecha_creacion");
            entity.Property(e => e.IdRol).HasColumnName("id_rol");
            entity.Property(e => e.NombreApellido).HasColumnName("nombre_apellido");
            entity.Property(e => e.Password).HasColumnName("password");
            entity.Property(e => e.Usuario1).HasColumnName("usuario");

            entity.HasOne(d => d.IdRolNavigation).WithMany(p => p.Usuarios)
                .HasForeignKey(d => d.IdRol)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_id_rol");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
