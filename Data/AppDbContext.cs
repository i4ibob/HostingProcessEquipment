using JuniorDotNetTestTaskServiceHostingProcessEquipment.Models;
using System.Collections.Generic;
using System.Reflection.Emit;
using Microsoft.EntityFrameworkCore;

namespace JuniorDotNetTestTaskServiceHostingProcessEquipment.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<ProductionFacility> ProductionFacilities { get; set; }
        public DbSet<ProcessEquipment> ProcessEquipments { get; set; }
        public DbSet<EquipmentPlacementContract> EquipmentPlacementContracts { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configuration for ProductionFacility
            modelBuilder.Entity<ProductionFacility>()
                .Property(p => p.StandardArea)
                .HasColumnType("decimal(18,2)")
                .HasDefaultValue(0)
                .IsRequired();

            modelBuilder.Entity<ProductionFacility>()
                .Property(p => p.Name)
                .HasMaxLength(200)
                .IsRequired();


            // Configuration for ProcessEquipment
            modelBuilder.Entity<ProcessEquipment>()
              .Property(p => p.Name)
              .HasMaxLength(200)
              .IsRequired();

            modelBuilder.Entity<ProcessEquipment>()
                .HasIndex(p => p.Code)
                .IsUnique();

            modelBuilder.Entity<ProcessEquipment>()
                .Property(p => p.Area)
                .HasColumnType("decimal(18,2)")
                .IsRequired();


            // Relationships for EquipmentPlacementContract
            modelBuilder.Entity<EquipmentPlacementContract>()
                .Property(e => e.Id)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<EquipmentPlacementContract>()
                .HasOne(c => c.ProductionFacility)
                .WithMany(f => f.Contracts)
                .HasForeignKey(c => c.ProductionFacilityId);

            modelBuilder.Entity<EquipmentPlacementContract>()
                .HasOne(c => c.ProcessEquipment)
                .WithMany(e => e.Contracts)
                .HasForeignKey(c => c.ProcessEquipmentId);

            base.OnModelCreating(modelBuilder);
        }
    }
}
