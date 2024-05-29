using System;
using System.Collections.Generic;
using Fleet_Management__Application.Models;
using Microsoft.EntityFrameworkCore;

namespace Fleet_Management__Application.Data;

public partial class DemoContext2 : DbContext
{
    public DemoContext2()
    {
    }

    public DemoContext2(DbContextOptions<DemoContext2> options)
        : base(options)
    {
    }

    public virtual DbSet<Circlegeofence> circlegeofences { get; set; }

    public virtual DbSet<Driver> drivers { get; set; }

    public virtual DbSet<Geofence> geofences { get; set; }

    public virtual DbSet<Polygongeofence> polygongeofences { get; set; }

    public virtual DbSet<Rectanglegeofence> rectanglegeofences { get; set; }

    public virtual DbSet<Routehistory> routehistories { get; set; }

    public virtual DbSet<Vehicle> vehicles { get; set; }

    public virtual DbSet<Vehiclesinformation> vehiclesinformations { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=localhost;Database=eyad_fms;Username=postgres;Password=123321");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Circlegeofence>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("circlegeofence_pkey");

            entity.HasOne(d => d.Geofence).WithMany(p => p.Circlegeofences).HasConstraintName("circlegeofence_geofenceid_fkey");
        });

        modelBuilder.Entity<Driver>(entity =>
        {
            entity.HasKey(e => e.Driverid).HasName("driver_pkey");
        });

        modelBuilder.Entity<Geofence>(entity =>
        {
            entity.HasKey(e => e.Geofenceid).HasName("geofences_pkey");
        });

        modelBuilder.Entity<Polygongeofence>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("polygongeofence_pkey");

            entity.HasOne(d => d.Geofence).WithMany(p => p.Polygongeofences).HasConstraintName("polygongeofence_geofenceid_fkey");
        });

        modelBuilder.Entity<Rectanglegeofence>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("rectanglegeofence_pkey");

            entity.HasOne(d => d.Geofence).WithMany(p => p.Rectanglegeofences).HasConstraintName("rectanglegeofence_geofenceid_fkey");
        });

        modelBuilder.Entity<Routehistory>(entity =>
        {
            entity.HasKey(e => e.Routehistoryid).HasName("routehistory_pkey");

            entity.HasOne(d => d.Vehicle).WithMany(p => p.Routehistories).HasConstraintName("routehistory_vehicleid_fkey");
        });

        modelBuilder.Entity<Vehicle>(entity =>
        {
            entity.HasKey(e => e.Vehicleid).HasName("vehicles_pkey");
        });

        modelBuilder.Entity<Vehiclesinformation>(entity =>
        {
            entity.HasKey(e => new { e.Id }).HasName("vehiclesinformations_pkey");

            entity.HasOne(d => d.Driver).WithMany(p => p.Vehiclesinformations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("vehiclesinformations_driverid_fkey");

            entity.HasOne(d => d.Vehicle).WithMany(p => p.Vehiclesinformations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("vehiclesinformations_vehicleid_fkey");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
