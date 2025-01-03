using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

public partial class CarsContext : DbContext
{
    public CarsContext()
    {
    }

    public CarsContext(DbContextOptions<CarsContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Accident> Accidents { get; set; }

    public virtual DbSet<Brand> Brands { get; set; }

    public virtual DbSet<Car> Cars { get; set; }

    public virtual DbSet<Mileage> Mileages { get; set; }

    public virtual DbSet<Owner> Owners { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseNpgsql("Server=localhost;userid=postgres;Password=1234;Database=cars;Port=1945");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Accident>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("accident");

            entity.Property(e => e.Date).HasColumnName("date");
            entity.Property(e => e.Number)
                .HasMaxLength(9)
                .HasColumnName("number");
            entity.Property(e => e.Region).HasColumnName("region");
            entity.Property(e => e.Type).HasColumnName("type");

            entity.HasOne(d => d.NumberNavigation).WithMany()
                .HasForeignKey(d => d.Number)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("accident_fk0");
        });

        modelBuilder.Entity<Brand>(entity =>
        {
            entity.HasKey(e => new { e.Title, e.FullTitle }).HasName("brands_pkey");

            entity.ToTable("brands");

            entity.HasIndex(e => e.Title, "brands_title_key").IsUnique();

            entity.Property(e => e.Title).HasColumnName("title");
            entity.Property(e => e.FullTitle).HasColumnName("full_title");
            entity.Property(e => e.Country).HasColumnName("country");
        });

        modelBuilder.Entity<Car>(entity =>
        {
            entity.HasKey(e => e.Number).HasName("car_pkey");

            entity.ToTable("car");

            entity.Property(e => e.Number)
                .HasMaxLength(9)
                .HasColumnName("number");
            entity.Property(e => e.Brand).HasColumnName("brand");
            entity.Property(e => e.Color).HasColumnName("color");
            entity.Property(e => e.Model).HasColumnName("model");

            entity.HasOne(d => d.BrandNavigation).WithMany(p => p.Cars)
                .HasPrincipalKey(p => p.Title)
                .HasForeignKey(d => d.Brand)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("car_fk1");
        });

        modelBuilder.Entity<Mileage>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("mileage");

            entity.Property(e => e.FixationDate).HasColumnName("fixationDate");
            entity.Property(e => e.FixationValue).HasColumnName("fixationValue");
            entity.Property(e => e.Number)
                .HasMaxLength(9)
                .HasColumnName("number");

            entity.HasOne(d => d.NumberNavigation).WithMany()
                .HasForeignKey(d => d.Number)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("mileage_fk0");
        });

        modelBuilder.Entity<Owner>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("owner");

            entity.Property(e => e.Name).HasColumnName("name");
            entity.Property(e => e.Number)
                .HasMaxLength(9)
                .HasColumnName("number");
            entity.Property(e => e.SecondName).HasColumnName("secondName");
            entity.Property(e => e.Surname).HasColumnName("surname");

            entity.HasOne(d => d.NumberNavigation).WithMany()
                .HasForeignKey(d => d.Number)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("owner_fk0");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
