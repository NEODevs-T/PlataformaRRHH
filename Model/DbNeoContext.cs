using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Inspecciones.Model;

public partial class DbNeoContext : DbContext
{
    public DbNeoContext()
    {
    }

    public DbNeoContext(DbContextOptions<DbNeoContext> options)
        : base(options)
    {
    }

    public virtual DbSet<ImaqPre> ImaqPres { get; set; }

    public virtual DbSet<Imaquina> Imaquinas { get; set; }

    public virtual DbSet<InspecDatum> InspecData { get; set; }

    public virtual DbSet<Inspeccion> Inspeccions { get; set; }

    public virtual DbSet<Ipreguntum> Ipregunta { get; set; }

    public virtual DbSet<ItipPre> ItipPres { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ImaqPre>(entity =>
        {
            entity.HasKey(e => e.IdMaqPre);

            entity.ToTable("IMaqPre", "Ins");

            entity.Property(e => e.Mpfecha)
                .HasColumnType("datetime")
                .HasColumnName("MPFecha");

            entity.HasOne(d => d.IdMaquinaNavigation).WithMany(p => p.ImaqPres)
                .HasForeignKey(d => d.IdMaquina)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_IMaqPre_IMaquina");

            entity.HasOne(d => d.IdPreguntaNavigation).WithMany(p => p.ImaqPres)
                .HasForeignKey(d => d.IdPregunta)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_IMaqPre_IPregunta");
        });

        modelBuilder.Entity<Imaquina>(entity =>
        {
            entity.HasKey(e => e.IdMaquina);

            entity.ToTable("IMaquina", "Ins");

            entity.Property(e => e.Mdescri)
                .IsUnicode(false)
                .HasColumnName("MDescri");
            entity.Property(e => e.Mestado).HasColumnName("MEstado");
            entity.Property(e => e.Mfecha)
                .HasColumnType("datetime")
                .HasColumnName("MFecha");
            entity.Property(e => e.Mnombre)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("MNombre");
        });

        modelBuilder.Entity<InspecDatum>(entity =>
        {
            entity.HasKey(e => e.IdInsData);

            entity.ToTable("InspecData", "Ins");

            entity.Property(e => e.Iddata).HasColumnName("IDData");
            entity.Property(e => e.Idobserv)
                .IsUnicode(false)
                .HasColumnName("IDObserv");

            entity.HasOne(d => d.IdInspecNavigation).WithMany(p => p.InspecData)
                .HasForeignKey(d => d.IdInspec)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_InspecData_Inspeccion");

            entity.HasOne(d => d.IdMaqPreNavigation).WithMany(p => p.InspecData)
                .HasForeignKey(d => d.IdMaqPre)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_InspecData_IMaqPre");
        });

        modelBuilder.Entity<Inspeccion>(entity =>
        {
            entity.HasKey(e => e.IdInspec);

            entity.ToTable("Inspeccion", "Ins");

            entity.Property(e => e.Iarea)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("IArea");
            entity.Property(e => e.Iequipo)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("IEquipo");
            entity.Property(e => e.Ifecha)
                .HasColumnType("datetime")
                .HasColumnName("IFecha");
            entity.Property(e => e.Ificha)
                .HasMaxLength(5)
                .IsUnicode(false)
                .HasColumnName("IFicha");
            entity.Property(e => e.Igrupo)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("IGrupo");
            entity.Property(e => e.Iturno)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("ITurno");
        });

        modelBuilder.Entity<Ipreguntum>(entity =>
        {
            entity.HasKey(e => e.IdPregunta);

            entity.ToTable("IPregunta", "Ins");

            entity.Property(e => e.Pdescri)
                .IsUnicode(false)
                .HasColumnName("PDescri");
            entity.Property(e => e.Pestado).HasColumnName("PEstado");
            entity.Property(e => e.Pfecha)
                .HasColumnType("datetime")
                .HasColumnName("PFecha");

            entity.HasOne(d => d.IdTipPreNavigation).WithMany(p => p.Ipregunta)
                .HasForeignKey(d => d.IdTipPre)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_IPregunta_ITipPre");
        });

        modelBuilder.Entity<ItipPre>(entity =>
        {
            entity.HasKey(e => e.IdTipPre);

            entity.ToTable("ITipPre", "Ins");

            entity.Property(e => e.Tpdescri)
                .IsUnicode(false)
                .HasColumnName("TPDescri");
            entity.Property(e => e.Tpestado).HasColumnName("TPEstado");
            entity.Property(e => e.Tpfecha)
                .HasColumnType("datetime")
                .HasColumnName("TPFecha");
            entity.Property(e => e.Tpnombre)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("TPNombre");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
