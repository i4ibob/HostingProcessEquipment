﻿// <auto-generated />
using JuniorDotNetTestTaskServiceHostingProcessEquipment.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace JuniorDotNetTestTaskServiceHostingProcessEquipment.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20241217181940_InitialDb")]
    partial class InitialDb
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("JuniorDotNetTestTaskServiceHostingProcessEquipment.Models.EquipmentPlacementContract", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("ProcessEquipmentId")
                        .HasColumnType("int");

                    b.Property<int>("ProductionFacilityId")
                        .HasColumnType("int");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ProcessEquipmentId");

                    b.HasIndex("ProductionFacilityId");

                    b.ToTable("EquipmentPlacementContracts");
                });

            modelBuilder.Entity("JuniorDotNetTestTaskServiceHostingProcessEquipment.Models.ProcessEquipment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<double>("Area")
                        .HasColumnType("float");

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("Code")
                        .IsUnique();

                    b.ToTable("ProcessEquipments");
                });

            modelBuilder.Entity("JuniorDotNetTestTaskServiceHostingProcessEquipment.Models.ProductionFacility", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("StandardArea")
                        .HasColumnType("float");

                    b.HasKey("Id");

                    b.HasIndex("Code")
                        .IsUnique();

                    b.ToTable("ProductionFacilities");
                });

            modelBuilder.Entity("JuniorDotNetTestTaskServiceHostingProcessEquipment.Models.EquipmentPlacementContract", b =>
                {
                    b.HasOne("JuniorDotNetTestTaskServiceHostingProcessEquipment.Models.ProcessEquipment", "ProcessEquipment")
                        .WithMany()
                        .HasForeignKey("ProcessEquipmentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("JuniorDotNetTestTaskServiceHostingProcessEquipment.Models.ProductionFacility", "ProductionFacility")
                        .WithMany("Contracts")
                        .HasForeignKey("ProductionFacilityId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ProcessEquipment");

                    b.Navigation("ProductionFacility");
                });

            modelBuilder.Entity("JuniorDotNetTestTaskServiceHostingProcessEquipment.Models.ProductionFacility", b =>
                {
                    b.Navigation("Contracts");
                });
#pragma warning restore 612, 618
        }
    }
}
