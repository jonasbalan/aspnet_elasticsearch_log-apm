﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using SamplesApi3.Infrastructure;

#nullable disable

namespace SamplesApi3.Migrations
{
    [DbContext(typeof(DoctorScheduleDbContext))]
    partial class DoctorScheduleDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.13")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("SamplesApi3.Model.Doctor", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Register")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Doctor");
                });

            modelBuilder.Entity("SamplesApi3.Model.DoctorAddress", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("City")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Complement")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Country")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid>("DoctorId")
                        .HasColumnType("uuid");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Neighborhood")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Number")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("State")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Street")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("ZIPCode")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("DoctorId");

                    b.ToTable("DoctorAddress");
                });

            modelBuilder.Entity("SamplesApi3.Model.DoctorSchedule", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("AddressId")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("DateEnd")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("DateInit")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("DoctorId")
                        .HasColumnType("uuid");

                    b.Property<int>("State")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("AddressId");

                    b.HasIndex("DoctorId");

                    b.ToTable("DoctorSchedule");
                });

            modelBuilder.Entity("SamplesApi3.Model.DoctorSchedulePatient", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid?>("DoctorId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("DoctorScheduleId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("PatientId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("SpecialtyId")
                        .HasColumnType("uuid");

                    b.Property<int>("State")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("DoctorId");

                    b.HasIndex("DoctorScheduleId");

                    b.HasIndex("PatientId");

                    b.HasIndex("SpecialtyId");

                    b.ToTable("DoctorSchedulePatient");
                });

            modelBuilder.Entity("SamplesApi3.Model.DoctorSpecialty", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("DoctorId")
                        .HasColumnType("uuid");

                    b.Property<Guid?>("DoctorScheduleId")
                        .HasColumnType("uuid");

                    b.Property<string>("Specialty")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("DoctorId");

                    b.HasIndex("DoctorScheduleId");

                    b.ToTable("DoctorSpecialty");
                });

            modelBuilder.Entity("SamplesApi3.Model.Patient", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("BornDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Patient");
                });

            modelBuilder.Entity("SamplesApi3.Model.DoctorAddress", b =>
                {
                    b.HasOne("SamplesApi3.Model.Doctor", "Doctor")
                        .WithMany("Adresses")
                        .HasForeignKey("DoctorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Doctor");
                });

            modelBuilder.Entity("SamplesApi3.Model.DoctorSchedule", b =>
                {
                    b.HasOne("SamplesApi3.Model.DoctorAddress", "Address")
                        .WithMany()
                        .HasForeignKey("AddressId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SamplesApi3.Model.Doctor", "Doctor")
                        .WithMany("Schedules")
                        .HasForeignKey("DoctorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Address");

                    b.Navigation("Doctor");
                });

            modelBuilder.Entity("SamplesApi3.Model.DoctorSchedulePatient", b =>
                {
                    b.HasOne("SamplesApi3.Model.Doctor", null)
                        .WithMany("SchedulePatients")
                        .HasForeignKey("DoctorId");

                    b.HasOne("SamplesApi3.Model.DoctorSchedule", "DoctorSchedule")
                        .WithMany()
                        .HasForeignKey("DoctorScheduleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SamplesApi3.Model.Patient", "Patient")
                        .WithMany()
                        .HasForeignKey("PatientId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SamplesApi3.Model.DoctorSpecialty", "Specialty")
                        .WithMany()
                        .HasForeignKey("SpecialtyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("DoctorSchedule");

                    b.Navigation("Patient");

                    b.Navigation("Specialty");
                });

            modelBuilder.Entity("SamplesApi3.Model.DoctorSpecialty", b =>
                {
                    b.HasOne("SamplesApi3.Model.Doctor", "Doctor")
                        .WithMany("Specialties")
                        .HasForeignKey("DoctorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SamplesApi3.Model.DoctorSchedule", null)
                        .WithMany("DoctorSpecialties")
                        .HasForeignKey("DoctorScheduleId");

                    b.Navigation("Doctor");
                });

            modelBuilder.Entity("SamplesApi3.Model.Doctor", b =>
                {
                    b.Navigation("Adresses");

                    b.Navigation("SchedulePatients");

                    b.Navigation("Schedules");

                    b.Navigation("Specialties");
                });

            modelBuilder.Entity("SamplesApi3.Model.DoctorSchedule", b =>
                {
                    b.Navigation("DoctorSpecialties");
                });
#pragma warning restore 612, 618
        }
    }
}
