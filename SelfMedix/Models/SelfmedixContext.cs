using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace SelfMedix.Models;

public partial class SelfmedixContext : DbContext
{
    public SelfmedixContext()
    {
    }

    public SelfmedixContext(DbContextOptions<SelfmedixContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Cita> Cita { get; set; }

    public virtual DbSet<Entidadmedica> Entidadmedicas { get; set; }

    public virtual DbSet<Historialmedico> Historialmedicos { get; set; }

    public virtual DbSet<Medico> Medicos { get; set; }

    public virtual DbSet<Paciente> Pacientes { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseMySQL("server=127.0.0.1;userid=root;password=;database=selfmedix;TreatTinyAsBoolean=False");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Cita>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("cita");

            entity.HasIndex(e => e.IdMedico, "id_cita_medico_idx");

            entity.HasIndex(e => new { e.IdPaciente, e.IdMedico }, "id_cita_paciente_idx");

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("id");
            entity.Property(e => e.Descripcion)
                .HasMaxLength(255)
                .HasDefaultValueSql("'NULL'");
            entity.Property(e => e.Estado).HasColumnType("int(11)");
            entity.Property(e => e.FechaCita).HasColumnType("datetime");
            entity.Property(e => e.FechaCreacion).HasColumnType("datetime");
            entity.Property(e => e.IdMedico)
                .HasColumnType("int(11)")
                .HasColumnName("idMedico");
            entity.Property(e => e.IdPaciente)
                .HasColumnType("int(11)")
                .HasColumnName("idPaciente");

            entity.HasOne(d => d.IdMedicoNavigation).WithMany(p => p.Cita)
                .HasForeignKey(d => d.IdMedico)
                .HasConstraintName("id_cita_medico");

            entity.HasOne(d => d.IdPacienteNavigation).WithMany(p => p.Cita)
                .HasForeignKey(d => d.IdPaciente)
                .HasConstraintName("id_cita_paciente");
        });

        modelBuilder.Entity<Entidadmedica>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("entidadmedica");

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("id");
            entity.Property(e => e.Direccion)
                .HasDefaultValueSql("'NULL'")
                .HasColumnType("text")
                .HasColumnName("direccion");
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .HasColumnName("nombre");

            entity.HasMany(d => d.IdMedicos).WithMany(p => p.IdEntidads)
                .UsingEntity<Dictionary<string, object>>(
                    "EntidadmedicaMedico",
                    r => r.HasOne<Medico>().WithMany()
                        .HasForeignKey("IdMedico")
                        .HasConstraintName("id_medico"),
                    l => l.HasOne<Entidadmedica>().WithMany()
                        .HasForeignKey("IdEntidad")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("id_entidad"),
                    j =>
                    {
                        j.HasKey("IdEntidad", "IdMedico").HasName("PRIMARY");
                        j.ToTable("entidadmedica_medico");
                        j.HasIndex(new[] { "IdMedico" }, "id_medico_idx");
                        j.IndexerProperty<int>("IdEntidad")
                            .ValueGeneratedOnAdd()
                            .HasColumnType("int(11)")
                            .HasColumnName("id_entidad");
                        j.IndexerProperty<int>("IdMedico")
                            .HasColumnType("int(11)")
                            .HasColumnName("id_medico");
                    });
        });

        modelBuilder.Entity<Historialmedico>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("historialmedico", tb => tb.HasComment("	"));

            entity.HasIndex(e => e.IdPaciente, "id_historial_paciente_idx");

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("id");
            entity.Property(e => e.Enfermedad)
                .HasMaxLength(255)
                .HasColumnName("enfermedad");
            entity.Property(e => e.Fechaingreso)
                .HasMaxLength(45)
                .HasColumnName("fechaingreso");
            entity.Property(e => e.IdPaciente)
                .HasColumnType("int(11)")
                .HasColumnName("idPaciente");
            entity.Property(e => e.Tratamiento)
                .HasMaxLength(255)
                .HasDefaultValueSql("'NULL'")
                .HasColumnName("tratamiento");

            entity.HasOne(d => d.IdPacienteNavigation).WithMany(p => p.Historialmedicos)
                .HasForeignKey(d => d.IdPaciente)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("id_historial_paciente");
        });

        modelBuilder.Entity<Medico>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("medico");

            entity.HasIndex(e => e.IdUsuarioMedico, "id_usuario_medico_idx");

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("id");
            entity.Property(e => e.IdUsuarioMedico)
                .HasColumnType("int(11)")
                .HasColumnName("id_usuario_medico");
            entity.Property(e => e.TituloCorto)
                .HasMaxLength(10)
                .HasColumnName("tituloCorto");

            entity.HasOne(d => d.IdUsuarioMedicoNavigation).WithMany(p => p.Medicos)
                .HasForeignKey(d => d.IdUsuarioMedico)
                .HasConstraintName("id_usuario_medico");
        });

        modelBuilder.Entity<Paciente>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("paciente");

            entity.HasIndex(e => e.IdUsuarioPaciente, "id_usuario_paciente_idx");

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("id");
            entity.Property(e => e.IdUsuarioPaciente)
                .HasColumnType("int(11)")
                .HasColumnName("id_usuario_paciente");

            entity.HasOne(d => d.IdUsuarioPacienteNavigation).WithMany(p => p.Pacientes)
                .HasForeignKey(d => d.IdUsuarioPaciente)
                .HasConstraintName("id_usuario_paciente");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("usuario");

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("id");
            entity.Property(e => e.Apellidos).HasMaxLength(255);
            entity.Property(e => e.Contrasenia)
                .HasMaxLength(45)
                .HasColumnName("contrasenia");
            entity.Property(e => e.Correo)
                .HasMaxLength(100)
                .HasColumnName("correo");
            entity.Property(e => e.FechaCreacion).HasColumnType("datetime");
            entity.Property(e => e.FechaElimina)
                .HasDefaultValueSql("'NULL'")
                .HasColumnType("datetime");
            entity.Property(e => e.FechaNacimiento).HasColumnType("datetime");
            entity.Property(e => e.Nombres).HasMaxLength(255);
            entity.Property(e => e.UrlImg)
                .HasDefaultValueSql("'NULL'")
                .HasColumnType("text");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
