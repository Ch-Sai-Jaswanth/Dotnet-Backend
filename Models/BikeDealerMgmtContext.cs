using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace BikeDealersProject.Models;

public partial class BikeDealerMgmtContext : DbContext
{
    public BikeDealerMgmtContext()
    {
    }

    public BikeDealerMgmtContext(DbContextOptions<BikeDealerMgmtContext> options)
        : base(options)
    {
    }

    public virtual DbSet<BikeStore> BikeStores { get; set; }

    public virtual DbSet<Dealer> Dealers { get; set; }

    public virtual DbSet<DealerMaster> DealerMasters { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {

        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<BikeStore>(entity =>
        {
            entity.HasKey(e => e.BikeId).HasName("PK__BikeStor__7DC817217A387B29");

            entity.Property(e => e.BikeId).ValueGeneratedNever();
            entity.Property(e => e.EngineCc).HasColumnName("EngineCC");
            entity.Property(e => e.Manufacturer)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ModelName)
                .HasMaxLength(30)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Dealer>(entity =>
        {
            entity.HasKey(e => e.DealerId).HasName("PK__Dealers__CA2F8EB2764699D7");

            entity.Property(e => e.DealerId).ValueGeneratedNever();
            entity.Property(e => e.Address)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.City)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.DealerName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.State)
                .HasMaxLength(20)
                .IsUnicode(false);
        });

        modelBuilder.Entity<DealerMaster>(entity =>
        {
            entity.HasKey(e => e.DealerMasterId).HasName("PK__DealerMa__70FBBB32CD96ECA9");

            entity.ToTable("DealerMaster");

            entity.Property(e => e.DealerMasterId).ValueGeneratedNever();
            entity.Property(e => e.DeliveryDate).HasColumnType("datetime");

            entity.HasOne(d => d.Bike).WithMany(p => p.DealerMasters)
                .HasForeignKey(d => d.BikeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DealerMaster_BikeStores");

            entity.HasOne(d => d.Dealer).WithMany(p => p.DealerMasters)
                .HasForeignKey(d => d.DealerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DealerMaster_Dealers");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Users__1788CC4C96D6E137");

            entity.Property(e => e.PasswordHash)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Role)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasDefaultValue("Admin");
            entity.Property(e => e.UserName)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
